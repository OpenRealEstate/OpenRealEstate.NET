using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using Shouldly;
using Features = OpenRealEstate.Core.Models.Features;
using LandFeatures = OpenRealEstate.Core.Models.Land.Features;

namespace OpenRealEstate.Services.RealEstateComAu
{
    public class ReaXmlTransmorgrifier : ITransmorgrifier
    {
        /// <summary>
        /// Converts some REA Xml data into a collection of parsed listings.
        /// </summary>
        /// <param name="data">Xml data to parse.</param>
        /// <param name="areBadCharactersRemoved">Option to remove/strip out bad characters.</param>
        /// <returns>Collection of listings.</returns>
        /// <remarks>The Xml data can either be a full REA Xml document (ie. &lt;propertyList/&gt; or a listing segment (ie. &lt;rental/&gt; / &lt;residential/&gt;.</remarks>
        public IList<Listing> Convert(string data, bool areBadCharactersRemoved = false)
        {
            data.ShouldNotBeNullOrEmpty();

            var validationErrorMessage = ValidateXmlString(data);
            if (!string.IsNullOrWhiteSpace(validationErrorMessage))
            {
                if (!areBadCharactersRemoved)
                {
                    throw new Exception(validationErrorMessage);
                }

                // Some bad data occurs, so lets clean any bad data out.
                data = RemoveInvalidXmlChars(data);
            }

            var elements = SplitReaXmlIntoElements(data);
            if (!elements.Any())
            {
                return null;
            }

            var listings = new ConcurrentBag<Listing>();

            Parallel.ForEach(elements, xml => listings.Add(ConvertFromReaXml(xml)));

            return listings.ToList();
        }

        private static string ValidateXmlString(string text)
        {
            text.ShouldNotBeNullOrEmpty();

            try
            {
                XmlConvert.VerifyXmlChars(text);
                return null;
            }
            catch (XmlException exception)
            {
                return string.Format(
                    "The REA Xml data provided contains some invalid characters. Line: {0}, Position: {1}. Error: {2} Suggested Solution: Either set the 'areBadCharactersRemoved' parameter to 'true' so invalid characters are removed automatically OR manually remove the errors from the file OR manually handle the error (eg. notify the people who sent you this data, that it contains bad data and they should clean it up.)",
                    exception.LineNumber,
                    exception.LinePosition,
                    exception.Message);
            }
            catch (Exception exception)
            {
                return string.Format("Failed to valid the xml string provided. Unknown error: {0}.", exception.Message);
            }
        }

        private static string RemoveInvalidXmlChars(string text)
        {
            text.ShouldNotBeNullOrEmpty();

            var validXmlChars = text.Where(XmlConvert.IsXmlChar).ToArray();
            return new string(validXmlChars);
        }

        private static IList<string> SplitReaXmlIntoElements(string xml)
        {
            xml.ShouldNotBeNullOrEmpty();

            var doc = XElement.Parse(xml).StripNameSpaces();

            // Do we have a full ReaXml document?
            if (doc.Name.LocalName.ToUpperInvariant() == "PROPERTYLIST")
            {
                var elements = new List<XElement>();
                elements.AddRange(doc.Descendants("residential").ToList());
                elements.AddRange(doc.Descendants("rental").ToList());
                elements.AddRange(doc.Descendants("land").ToList());

                return elements.Select(x => x.ToString()).ToList();
            }

            // Do we have a single listing *segment* ?
            if (doc.Name.LocalName.ToUpperInvariant() == "RESIDENTIAL" ||
                doc.Name.LocalName.ToUpperInvariant() == "RENTAL" ||
                doc.Name.LocalName.ToUpperInvariant() == "LAND")
            {
                return new List<string> {doc.ToString()};
            }

            var errorMessage =
                string.Format(
                    "Unable to parse the xml data provided. Currently, only a <propertyList/> or listing segments <residential/> / <rental/> / <land/>. Root node found: '{0}'.",
                    doc.Name.LocalName);
            throw new Exception(errorMessage);
        }

        private static Listing ConvertFromReaXml(string xml)
        {
            xml.ShouldNotBeNullOrEmpty();

            var doc = XElement.Parse(xml);

            // Determine the category, so we know why type of listing we need to create.
            var categoryType = doc.Name.ToCategoryType();

            // We can only handle a subset of all the category types.
            var listing = CreateListing(categoryType);
            if (listing == null)
            {
                // TODO: Add logging message.
                return null;
            }

            // Extract common data.
            ExtractCommonData(listing, doc);

            // Extract specific data.
            if (listing is ResidentialListing)
            {
                ExtractResidentialData(listing as ResidentialListing, doc);
            }

            if (listing is RentalListing)
            {
                ExtractRentalData(listing as RentalListing, doc);
            }

            if (listing is LandListing)
            {
                ExtractLandData(listing as LandListing, doc);
            }

            return listing;
        }

        private static Listing CreateListing(CategoryType categoryType)
        {
            Listing listing = null;

            switch (categoryType)
            {
                case CategoryType.Sale:
                    listing = new ResidentialListing();
                    break;
                case CategoryType.Rent:
                    listing = new RentalListing();
                    break;
                case CategoryType.Land:
                    listing = new LandListing();
                    break;
                default:
                    // Not sure if we should do some logging here?
                    listing = null;
                    break;
            }

            return listing;
        }

        private static DateTime? ToDateTime(string reaDateTime)
        {
            // REFERENCE: http://reaxml.realestate.com.au/docs/reaxml1-xml-format.html#datetime
            /*
                YYYY-MM-DD
                YYYY-MM-DD-hh:mm
                YYYY-MM-DD-hh:mm:ss
                YYYY-MM-DDThh:mm
                YYYY-MM-DDThh:mm:ss
                YYYYMMDD
                YYYYMMDD-hhmm
                YYYYMMDD-hhmmss
                YYYYMMDDThhmm
                YYYYMMDDThhmmss
             */
            var formats = new[]
            {
                "yyyy-MM-dd",
                "yyyy-MM-dd-HH:mm:ss",
                "yyyy-MM-ddTHH:mm:ss",
                "yyyy-MM-dd-HH:mm:",
                "yyyy-MM-ddTHH:mm:ss",
                "yyyyMMdd-HHmmss",
                "yyyyMMddTHHmmss",
                "yyyyMMdd-HHmm",
                "yyyyMMddTHHmm",
                "yyyyMMdd",
                "o",
                "s"
            };


            DateTime result;
            if (DateTime.TryParseExact(reaDateTime,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out result))
            {
                return result;
            }

            if (DateTime.TryParse(reaDateTime, out result))
            {
                return result;
            }

            return null;
        }

        #region Common listing methods

        private static void ExtractCommonData(Listing listing, XElement xElement)
        {
            listing.ShouldNotBe(null);
            xElement.ShouldNotBe(null);

            listing.UpdatedOn = ParseReaDateTime(xElement.AttributeValue("modTime"));

            // We have no idea if this was created before this date, but we need to set a date
            // so we'll default it to this.
            listing.CreatedOn = listing.UpdatedOn;

            listing.AgencyId = xElement.Value("agentID");
            listing.Id = xElement.Value("uniqueID");
            var status = xElement.AttributeValueOrDefault("status");
            if (!string.IsNullOrWhiteSpace(status))
            {
                listing.StatusType = StatusTypeHelpers.ToStatusType(status);
            }

            listing.Title = xElement.ValueOrDefault("headline");
            listing.Description = xElement.ValueOrDefault("description");

            listing.Address = ExtractAddress(xElement);
            listing.Agents = ExtractAgent(xElement);
            listing.Inspections = ExtractInspectionTimes(xElement);
            listing.Images = ExtractImages(xElement);
            listing.FloorPlans = ExtractFloorPlans(xElement);
        }

        private static Address ExtractAddress(XElement xElement)
        {
            xElement.ShouldNotBe(null);

            var addressElement = xElement.Element("address");
            if (addressElement == null)
            {
                return null;
            }

            var address = new Address();

            // Land and CommericalLand should only provide lot numbers. 
            var lotNumber = addressElement.ValueOrDefault("lotNumber");
            var subNumber = addressElement.ValueOrDefault("subNumber");
            address.StreetNumber = string.Format("{0}{1}{2}{3}{4}",
                string.IsNullOrWhiteSpace(lotNumber)
                    ? string.Empty
                    : lotNumber.IndexOf("lot", StringComparison.InvariantCultureIgnoreCase) > 0
                        ? lotNumber
                        : string.Format("LOT {0}", lotNumber),
                !string.IsNullOrWhiteSpace(lotNumber) &&
                !string.IsNullOrWhiteSpace(subNumber)
                    ? " "
                    : string.Empty,
                string.IsNullOrWhiteSpace(subNumber)
                ? string.Empty
                : subNumber,
                string.IsNullOrEmpty(lotNumber) &&
                string.IsNullOrEmpty(subNumber)
                    ? string.Empty
                    : "/",
                addressElement.ValueOrDefault("streetNumber"));

            address.Street = addressElement.ValueOrDefault("street");
            address.Suburb = addressElement.ValueOrDefault("suburb");
            address.State = addressElement.ValueOrDefault("state");

            // REA Xml Rule: Country is ommited == default to Australia.
            // Reference: http://reaxml.realestate.com.au/docs/reaxml1-xml-format.html#country
            var country = addressElement.ValueOrDefault("country");
            address.CountryIsoCode = !string.IsNullOrEmpty(country)
                ? ConvertCountryToIsoCode(country)
                : "AU";

            address.Postcode = addressElement.ValueOrDefault("postcode");
            address.IsStreetDisplayed = addressElement.AttributeBoolValueOrDefault("display");

            return address;
        }

        private static IList<ListingAgent> ExtractAgent(XElement xElement)
        {
            xElement.ShouldNotBe(null);

            var agentElements = xElement.Elements("listingAgent").ToArray();
            if (!agentElements.Any())
            {
                return null;
            }

            var agents = new List<ListingAgent>();

            foreach (var agentElement in agentElements)
            {
                var agent = new ListingAgent
                {
                    Name = agentElement.ValueOrDefault("name")
                };

                var orderValue = agentElement.AttributeValueOrDefault("id");
                int order = 0;
                if (!string.IsNullOrWhiteSpace(orderValue) &&
                    int.TryParse(orderValue, out order))
                {
                    agent.Order = order;
                }

                var email = agentElement.ValueOrDefault("email");
                agent.Communications = new List<Communication>();
                if (!string.IsNullOrWhiteSpace(email))
                {
                    agent.Communications.Add(new Communication
                    {
                        CommunicationType = CommunicationType.Email,
                        Details = email
                    });
                }

                var phoneMobile = agentElement.ValueOrDefault("telephone", "type", "mobile");
                if (!string.IsNullOrWhiteSpace(phoneMobile))
                {
                    agent.Communications.Add(new Communication
                    {
                        CommunicationType = CommunicationType.Landline,
                        Details = phoneMobile
                    });
                }

                var phoneOffice = agentElement.ValueOrDefault("telephone", "type", "BH");
                if (!string.IsNullOrWhiteSpace(phoneOffice))
                {
                    agent.Communications.Add(new Communication
                    {
                        CommunicationType = CommunicationType.Landline,
                        Details = phoneOffice
                    });
                }

                // Some listings have this element but no data provided. :(
                // So we don't add 'emtpy' agents.
                if (!string.IsNullOrWhiteSpace(agent.Name) &&
                    agent.Communications.Any())
                {
                    agents.Add(agent);
                }
            }

            var counter = 0;
            return agents.Any()
                ? agents
                    .OrderBy(x => x.Order)
                    .Select(x => new ListingAgent
                    {
                        Name = x.Name,
                        Order = ++counter,
                        Communications = x.Communications
                    })
                    .ToList()
                : null;
        }

        private static Features ExtractFeatures(XElement xElement)
        {
            xElement.ShouldNotBe(null);

            var featuresElement = xElement.Element("features");
            if (featuresElement == null)
            {
                return null;
            }

            // No one really knows what to use for cars. Garages? Carspaces?
            // We'll use carspaces over garages.
            var carspaces = featuresElement.ByteValueOrDefault("carports");
            var garages = featuresElement.ByteValueOrDefault("garages");

            var features = new Features
            {
                Bedrooms = featuresElement.ByteValueOrDefault("bedrooms"),
                Bathrooms = featuresElement.ByteValueOrDefault("bathrooms"),
                CarSpaces = carspaces > 0
                    ? carspaces
                    : garages > 0
                        ? garages
                        : 0
            };

            return features;
        }

        private static IList<Inspection> ExtractInspectionTimes(XElement xElement)
        {
            xElement.ShouldNotBe(null);

            var inspectionTimes = xElement.Element("inspectionTimes");
            if (inspectionTimes == null)
            {
                return null;
            }

            var inspectionElements = inspectionTimes.Elements("inspection").ToList();
            if (!inspectionElements.Any())
            {
                return null;
            }

            var inspections = new List<Inspection>();

            foreach (var inspectionElement in inspectionElements)
            {
                // -Some- xml data only contains empty inspects. (eg. RentalExpress).
                if (inspectionElement == null ||
                    inspectionElement.IsEmpty ||
                    string.IsNullOrWhiteSpace(inspectionElement.Value))
                {
                    continue;
                }

                // Only the following format is accepted as valid: DD-MON-YYYY hh:mm[am|pm] to hh:mm[am|pm]
                // REF: http://reaxml.realestate.com.au/docs/reaxml1-xml-format.html#inspection
                var data = inspectionElement.Value.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                DateTime inspectionStartsOn, inspectionEndsOn;

                DateTime.TryParse(string.Format("{0} {1}", data[0], data[1]), out inspectionStartsOn);
                DateTime.TryParse(string.Format("{0} {1}", data[0], data[3]), out inspectionEndsOn);

                if (inspectionStartsOn == DateTime.MinValue ||
                    inspectionEndsOn == DateTime.MinValue)
                {
                    throw new Exception("Inspection element has an invalid Date/Time value. Element: " +
                                        inspectionElement);
                }

                var newInspection = new Inspection
                {
                    OpensOn = inspectionStartsOn,
                    ClosesOn = inspectionEndsOn == inspectionStartsOn
                        ? (DateTime?) null
                        : inspectionEndsOn
                };

                // Just to be safe, lets make sure do get dupes.
                if (!inspections.Contains(newInspection))
                {
                    inspections.Add(newInspection);
                }
            }

            return inspections.Any()
                ? inspections.OrderBy(x => x.OpensOn).ToList()
                : null;
        }

        private static IList<Media> ExtractImages(XElement xElement)
        {
            xElement.ShouldNotBe(null);

            var imageElement = xElement.Element("images") ?? xElement.Element("objects");
            if (imageElement == null)
            {
                return null;
            }

            var imagesElements = imageElement.Elements("img").ToArray();
            if (!imagesElements.Any())
            {
                return null;
            }

            var images = (from x in imagesElements
                let url = x.AttributeValueOrDefault("url")
                let order = x.AttributeValueOrDefault("id")
                where !string.IsNullOrWhiteSpace(url)
                select new Media
                {
                    Url = url,
                    Order = ConvertImageOrderToNumber(order)
                }).ToList();

            return images.Any() ? images : null;
        }

        private static IList<Media> ExtractFloorPlans(XElement xElement)
        {
            xElement.ShouldNotBe(null);

            var objectElement = xElement.Element("objects");
            if (objectElement == null)
            {
                return null;
            }

            var floorPlanElements = objectElement.Elements("floorplan").ToArray();
            if (!floorPlanElements.Any())
            {
                return null;
            }

            var floorPlans = (from x in floorPlanElements
                let url = x.AttributeValueOrDefault("url")
                let order = x.AttributeValueOrDefault("id")
                where !string.IsNullOrWhiteSpace(url)
                select new Media
                {
                    Url = url,
                    Order = System.Convert.ToInt32(order)
                }).ToList();

            return floorPlans.Any() ? floorPlans : null;
        }

        private static PropertyType ExtractResidentialAndRentalPropertyType(XElement xElement)
        {
            var propertyType = PropertyType.Unknown;

            var category = xElement.ValueOrDefault("category", "name");
            if (!string.IsNullOrWhiteSpace(category))
            {
                propertyType = PropertyTypeHelpers.ToPropertyType(category);
            }

            return propertyType;
        }

        private static SalePricing ExtractSalePricing(XElement xElement)
        {
            xElement.ShouldNotBe(null);

            var salePricing = new SalePricing
            {
                SalePrice = xElement.DecimalValueOrDefault("price")
            };

            // Selling data.

            var salePriceText = xElement.ValueOrDefault("priceView");
            var displayAttributeValue = xElement.ValueOrDefault("priceView", "display");
            var isDisplay = string.IsNullOrWhiteSpace(displayAttributeValue) ||
                            displayAttributeValue.ParseYesNoToBool();
            salePricing.SalePriceText = isDisplay
                ? salePriceText
                : string.IsNullOrWhiteSpace(salePriceText)
                    ? "Address Witheld"
                    : salePriceText;

            var isUnderOffer = xElement.ValueOrDefault("underOffer", "value");
            salePricing.IsUnderOffer = !string.IsNullOrWhiteSpace(isUnderOffer) &&
                                       isUnderOffer.ParseYesNoToBool();


            // Sold data.
            var soldDetails = xElement.Element("soldDetails");
            if (soldDetails != null)
            {
                salePricing.SoldPrice = soldDetails.DecimalValueOrDefault("price");
                var soldDisplayAttribute = soldDetails.ValueOrDefault("price", "display");
                salePricing.IsSoldPriceVisibile = string.IsNullOrWhiteSpace(soldDisplayAttribute) ||
                                                  soldDisplayAttribute.ParseYesNoToBool();

                var soldOnText = soldDetails.ValueOrDefault("date");
                if (!string.IsNullOrWhiteSpace(soldOnText))
                {
                    salePricing.SoldOn = ToDateTime(soldOnText);
                }
            }

            return salePricing;
        }

        private static DateTime? ExtractAuction(XElement xElement)
        {
            xElement.ShouldNotBe(null);

            var auction = xElement.ValueOrDefault("auction");

            // NOTE: The REA documentation is vague as to the 100% specification on this.
            // So i'm going to assume the following (in order)
            // 1. <auction>date-time-in-here</auction>
            // 2. <auction date="date-time-in-here"></auction>
            // ** YET ANOTHER FRICKING EXAMPLE OF WHY THIS SCHEMA AND XML ARE F'ING CRAP **
            if (string.IsNullOrWhiteSpace(auction))
            {
                auction = xElement.ValueOrDefault("auction", "date");
            }

            return (!string.IsNullOrWhiteSpace(auction))
                ? ToDateTime(auction)
                : null;
        }

        /// <summary>
        /// REA Specific DateTime parsing.
        /// </summary>
        private static DateTime ParseReaDateTime(string someDateTime)
        {
            someDateTime.ShouldNotBeNullOrEmpty();

            DateTime resultDateTime;

            if (!DateTime.TryParse(someDateTime, out resultDateTime))
            {
                DateTime.TryParseExact(someDateTime.Trim(), "yyyy-MM-dd-H:mm:ss", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out resultDateTime);
            }

            if (resultDateTime == DateTime.MinValue)
            {
                DateTime.TryParseExact(someDateTime.Trim(), "yyyyMMdd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out resultDateTime);
            }

            return resultDateTime;
        }

        private static string ConvertCountryToIsoCode(string country)
        {
            country.ShouldNotBeNullOrEmpty();

            switch (country.ToUpperInvariant())
            {
                case "AU":
                case "AUS":
                case "AUSTRALIA":
                    return "AU";
                case "NZ":
                case "NEW ZEALAND":
                    return "NZ";
                default:
                    throw new ArgumentOutOfRangeException("country",
                        string.Format("Country '{0}' is unhandled - not sure of the ISO Code to use.", country));
            }
        }

        private static int ConvertImageOrderToNumber(string imageOrder)
        {
            var character = imageOrder.ToUpperInvariant().First();

            // 65 == 'A'. But we need 'm' to be 1, so we have to do some funky shit.
            // A == 65 - 63 == 2
            // B == 66 - 63 == 3

            return character == 'M' ? 1 : character - 63;
        }

        #endregion

        #region Residential Listing methods

        private static void ExtractResidentialData(ResidentialListing residentialListing, XElement xElement)
        {
            residentialListing.ShouldNotBe(null);
            xElement.ShouldNotBe(null);

            residentialListing.PropertyType = ExtractResidentialAndRentalPropertyType(xElement);
            residentialListing.Pricing = ExtractSalePricing(xElement);
            residentialListing.AuctionOn = ExtractAuction(xElement);
            residentialListing.Features = ExtractFeatures(xElement);
        }

        #endregion

        #region Rental Listing Methods

        public static void ExtractRentalData(RentalListing rentalListing, XElement xElement)
        {
            rentalListing.ShouldNotBe(null);
            xElement.ShouldNotBe(null);

            var dateAvailble = xElement.ValueOrDefault("dateAvailable");
            if (!string.IsNullOrWhiteSpace(dateAvailble))
            {
                rentalListing.AvailableOn = ToDateTime(dateAvailble);
            }

            rentalListing.PropertyType = ExtractResidentialAndRentalPropertyType(xElement);
            rentalListing.Pricing = ExtractRentalPricing(xElement);
            rentalListing.Features = ExtractFeatures(xElement);
        }

        public static RentalPricing ExtractRentalPricing(XElement xElement)
        {
            xElement.ShouldNotBe(null);


            // Quote: There can be multiple rent elements if you wish to specify a price for both monthly and weekly. 
            //        However, at least one of the rent elements must be for a weekly period.
            // Result: FML :(
            var rentElements = xElement.Elements("rent").ToArray();
            if (!rentElements.Any())
            {
                return null;
            }

            // We will only use the WEEKLY one.
            var rentalPricing = new RentalPricing();
            foreach (var re in rentElements)
            {
                var frequency = re.AttributeValueOrDefault("period");
                if (string.IsNullOrWhiteSpace(frequency))
                {
                    continue;
                }

                frequency = frequency.ToUpperInvariant();
                if (frequency != "WEEK" &&
                    frequency != "WEEKLY")
                {
                    continue;
                }

                var rentalPrice = re.Value;
                decimal value;
                if (!string.IsNullOrWhiteSpace(rentalPrice) &&
                    Decimal.TryParse(rentalPrice, out value))
                {
                    rentalPricing.RentalPrice = value;
                }

                break;
            }

            rentalPricing.RentalPriceText = xElement.ValueOrDefault("priceView");
            rentalPricing.Bond = xElement.DecimalValueOrDefault("bond");

            return rentalPricing;
        }

        #endregion

        #region Land Listing Methods

        private static void ExtractLandData(LandListing landListing, XElement xElement)
        {
            landListing.ShouldNotBe(null);
            xElement.ShouldNotBe(null);

            landListing.CategoryType = ExtractLandCategoryType(xElement);
            landListing.Pricing = ExtractSalePricing(xElement);
            landListing.AuctionOn = ExtractAuction(xElement);
            landListing.Estate = ExtractLandEstate(xElement);
            landListing.Features = ExtractLandFeatures(xElement);
            landListing.Details = ExtractLandDetails(xElement);
            landListing.AuctionOn = ExtractAuction(xElement);
        }

        private static Core.Models.Land.CategoryType ExtractLandCategoryType(XElement xElement)
        {
            var categoryType = Core.Models.Land.CategoryType.Unknown;

            var category = xElement.ValueOrDefault("landCategory", "name");
            if (!string.IsNullOrWhiteSpace(category))
            {
                categoryType = CategoryTypeHelpers.ToCategoryType(category);
            }

            return categoryType;
        }

        private static LandEstate ExtractLandEstate(XElement xElement)
        {
            xElement.ShouldNotBe(null);

            var estateElement = xElement.Element("estate");
            if (estateElement == null)
            {
                return null;
            }

            return new LandEstate
            {
                Name = estateElement.ValueOrDefault("name"),
                Stage = estateElement.ValueOrDefault("stage")
            };
        }

        private static LandFeatures ExtractLandFeatures(XElement xElement)
        {
            xElement.ShouldNotBe(null);

            var featuresElement = xElement.Element("features");
            if (featuresElement == null)
            {
                return null;
            }

            return new LandFeatures
            {
                FullyFenced = featuresElement.BoolValueOrDefault("fullyFenced")
            };
        }

        private static Details ExtractLandDetails(XElement xElement)
        {
            xElement.ShouldNotBe(null);

            var landDetailsElement = xElement.Element("landDetails");
            if (landDetailsElement == null)
            {
                return null;
            }

            var areaValue = landDetailsElement.DecimalValueOrDefault("area");
            var areaType = landDetailsElement.ValueOrDefault("area", "unit");
            var frontageValue = landDetailsElement.DecimalValueOrDefault("frontage");
            var frontageType = landDetailsElement.ValueOrDefault("frontage", "unit");
            var depthValue = landDetailsElement.DecimalValueOrDefault("depth");
            var depthType = landDetailsElement.ValueOrDefault("depth", "unit");
            var depthSide = landDetailsElement.ValueOrDefault("depth", "side"); 

            var details = new Details
            {
                CrossOver = landDetailsElement.ValueOrDefault("crossOver", "value")
            };

            if (areaValue > 0)
            {
                details.Area = new UnitOfMeasure
                {
                    Value = areaValue,
                    Type = string.IsNullOrWhiteSpace(areaType)
                        ? "Total"
                        : areaType
                };
            }

            if (frontageValue > 0)
            {
                details.Frontage = new UnitOfMeasure
                {
                    Value = frontageValue,
                    Type = string.IsNullOrWhiteSpace(frontageType)
                        ? "Total"
                        : frontageType
                };
            }

            if (depthValue > 0)
            {
                details.Depth = new Depth
                {
                    UnitOfMeasure = new UnitOfMeasure
                    {
                        Value = depthValue,
                        Type = string.IsNullOrWhiteSpace(depthType)
                            ? "Total"
                            : depthType
                    },
                    Side = depthSide
                };
            }

            return details;
        }

        #endregion
    }
}