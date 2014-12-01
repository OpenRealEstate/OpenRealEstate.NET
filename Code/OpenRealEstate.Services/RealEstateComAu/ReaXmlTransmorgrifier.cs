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
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;
using Shouldly;
using CategoryTypeHelpers = OpenRealEstate.Core.Models.Land.CategoryTypeHelpers;

namespace OpenRealEstate.Services.RealEstateComAu
{
    public class ReaXmlTransmorgrifier : ITransmorgrifier
    {
        private static readonly IList<string> ValidRootNodes = new List<string> { "propertyList", "residential", "rental", "rural", "land" };
        private CultureInfo _defaultCultureInfo;

        /// <summary>
        /// Converts some REA Xml data into a collection of parsed listings.
        /// </summary>
        /// <param name="data">Xml data to parse.</param>
        /// <param name="areBadCharactersRemoved">Option to remove/strip out bad characters.</param>
        /// <returns>Collection of listings.</returns>
        /// <remarks>The Xml data can either be a full REA Xml document (ie. &lt;propertyList/&gt; or a listing segment (ie. &lt;rental/&gt; / &lt;residential/&gt;.</remarks>
        public ConvertToResult ConvertTo(string data, bool areBadCharactersRemoved = false)
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

            // Now split it up into the known listing types.
            var elements = SplitReaXmlIntoElements(data);
            if (!elements.KnownXmlData.Any() &&
                !elements.UnknownXmlData.Any())
            {
                return null;
            }

            // Finally, we convert each segment into a listing.
            var listings = new ConcurrentBag<ListingResult>();
            Parallel.ForEach(elements.KnownXmlData, element =>
                listings.Add(new ListingResult
                {
                    Listing = ConvertFromReaXml(element, DefaultCultureInfo),
                    SourceData = element.ToString()
                }));

            return new ConvertToResult
            {
                Listings = listings.ToList(),
                UnhandledData = elements.UnknownXmlData != null &&
                                elements.UnknownXmlData.Any()
                    ? elements.UnknownXmlData.Select(x => x.ToString()).ToList()
                    : null
            };
        }

        public CultureInfo DefaultCultureInfo
        {
            get { return _defaultCultureInfo ?? new CultureInfo("en-au"); }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _defaultCultureInfo = value;
            }
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

        private static void EnsureXmlHasRootNode(ref XDocument document)
        {
            document.ShouldNotBe(null);

            var rootNode = document.Root == null
                ? null
                : document.Root.Name.LocalName;

            if (string.IsNullOrWhiteSpace(rootNode) ||
                !ValidRootNodes.Contains(document.Root.Name.LocalName))
            {
                var errorMessage =
                    string.Format(
                        "Unable to parse the xml data provided. Currently, only a <propertyList/> or listing segments <residential/> / <rental/> / <land/> / <rural/>. Root node found: '{0}'.",
                        document.Root == null
                            ? "-no root node"
                            : document.Root.Name.LocalName);
                throw new Exception(errorMessage);
            }

            // Lets make sure our document has a propertyList root node.
            if (rootNode != "propertyList")
            {
                document = new XDocument(new XElement("propertyList", document.Root));
            }
        }

        private static SplitElementResult SplitReaXmlIntoElements(string xml)
        {
            xml.ShouldNotBeNullOrEmpty();

            var document = XDocument.Parse(xml);

            // Prepare the xml data we're given.
            EnsureXmlHasRootNode(ref document);

            var knownNodes = new[]
            {
                "residential",
                "rental",
                "land",
                "rural"
            };

            return document.Root == null
                ? null
                : new SplitElementResult
                {
                    KnownXmlData = document.Root.Elements()
                        .Where(
                            x =>
                                knownNodes.Any(
                                    node =>
                                        string.Compare(node, x.Name.LocalName, true, CultureInfo.InvariantCulture) == 0))
                        .ToList(),
                    UnknownXmlData = document.Root.Elements()
                        .Where(
                            x =>
                                knownNodes.All(
                                    node =>
                                        string.Compare(node, x.Name.LocalName, true, CultureInfo.InvariantCulture) != 0))
                        .ToList()
                };
        }

        private static Listing ConvertFromReaXml(XElement document, CultureInfo cultureInfo)
        {
            document.ShouldNotBe(null);

            // Determine the category, so we know why type of listing we need to create.
            var categoryType = document.Name.ToCategoryType();

            // We can only handle a subset of all the category types.
            var listing = CreateListing(categoryType);
            if (listing == null)
            {
                // TODO: Add logging message.
                return null;
            }

            // Extract common data.
            ExtractCommonData(listing, document);

            // Extract specific data.
            if (listing is ResidentialListing)
            {
                ExtractResidentialData(listing as ResidentialListing, document, cultureInfo);
            }

            if (listing is RentalListing)
            {
                ExtractRentalData(listing as RentalListing, document, cultureInfo);
            }

            if (listing is LandListing)
            {
                ExtractLandData(listing as LandListing, document, cultureInfo);
            }

            if (listing is RuralListing)
            {
                ExtractRuralData(listing as RuralListing, document, cultureInfo);
            }

            return listing;
        }

        private static Listing CreateListing(CategoryType categoryType)
        {
            Listing listing;

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
                case CategoryType.Rural:
                    listing = new RuralListing();
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

        private static void ExtractCommonData(Listing listing, XElement document)
        {
            listing.ShouldNotBe(null);
            document.ShouldNotBe(null);

            listing.UpdatedOn = ParseReaDateTime(document.AttributeValue("modTime"));

            // We have no idea if this was created before this date, but we need to set a date
            // so we'll default it to this.
            listing.CreatedOn = listing.UpdatedOn;

            listing.AgencyId = document.ValueOrDefault("agentID");
            listing.Id = document.ValueOrDefault("uniqueID");
            var status = document.AttributeValueOrDefault("status");
            if (!string.IsNullOrWhiteSpace(status))
            {
                listing.StatusType = StatusTypeHelpers.ToStatusType(status);
            }

            listing.Title = document.ValueOrDefault("headline");
            listing.Description = document.ValueOrDefault("description");

            listing.Address = ExtractAddress(document);
            listing.Agents = ExtractAgent(document);
            listing.Inspections = ExtractInspectionTimes(document);
            listing.Images = ExtractImages(document);
            listing.FloorPlans = ExtractFloorPlans(document);
            listing.LandDetails = ExtractLandDetails(document);
        }

        private static Address ExtractAddress(XElement document)
        {
            document.ShouldNotBe(null);

            var addressElement = document.Element("address");
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

            var isStreetDisplayedText = addressElement.AttributeValueOrDefault("display");
            address.IsStreetDisplayed = string.IsNullOrWhiteSpace(isStreetDisplayedText) ||
                                        addressElement.AttributeBoolValueOrDefault("display");

            return address;
        }

        private static IList<ListingAgent> ExtractAgent(XElement document)
        {
            document.ShouldNotBe(null);

            var agentElements = document.Elements("listingAgent").ToArray();
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
                        CommunicationType = CommunicationType.Mobile,
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

        private static Features ExtractFeatures(XElement document)
        {
            document.ShouldNotBe(null);

            var featuresElement = document.Element("features");
            if (featuresElement == null)
            {
                return null;
            }

            // No one really knows what to use for cars. Garages? Carspaces?
            // We'll use carspaces over garages.
            var carspaces = featuresElement.ByteValueOrDefault("carports");
            var garages = featuresElement.ByteValueOrDefault("garages");

            // NOTE: Bedrooms can be a number -or- the value 'STUDIO'.
                //       YES - where a number is the logical value, they can now have a string.
                //       So, if the value is a string, like STUDIO (or anything else), then the
                //       value will be returned as ZERO.
            var bedroomsValue = featuresElement.ValueOrDefault("bedrooms");
            var bedrooms = !string.IsNullOrWhiteSpace(bedroomsValue) &&
                           bedroomsValue.Equals("studio", StringComparison.OrdinalIgnoreCase)
                ? 0
                : featuresElement.ByteValueOrDefault("bedrooms");

            var features = new Features
            {
                Bedrooms = bedrooms,
                Bathrooms = featuresElement.ByteValueOrDefault("bathrooms"),
                CarSpaces = carspaces > 0
                    ? carspaces
                    : garages > 0
                        ? garages
                        : 0
            };

            return features;
        }

        private static IList<Inspection> ExtractInspectionTimes(XElement document)
        {
            document.ShouldNotBe(null);

            var inspectionTimes = document.Element("inspectionTimes");
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

                if (data.Length < 4)
                {
                    throw new Exception("Inspection element has an invald Date/Time value. Element: " +
                                        inspectionElement);
                }

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

        private static IList<Media> ExtractImages(XElement document)
        {
            document.ShouldNotBe(null);

            var imageElement = document.Element("images") ?? document.Element("objects");
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
                where !string.IsNullOrWhiteSpace(url) &&
                    !string.IsNullOrWhiteSpace(order)
                select new Media
                {
                    Url = url,
                    Order = ConvertImageOrderToNumber(order)
                }).ToList();

            return images.Any() ? images : null;
        }

        private static IList<Media> ExtractFloorPlans(XElement document)
        {
            document.ShouldNotBe(null);

            var objectElement = document.Element("objects");
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
                    Order = Convert.ToInt32(order)
                }).ToList();

            return floorPlans.Any() ? floorPlans : null;
        }

        private static PropertyType ExtractResidentialAndRentalPropertyType(XElement document)
        {
            var propertyType = PropertyType.Unknown;

            var category = document.ValueOrDefault("category", "name");
            if (!string.IsNullOrWhiteSpace(category))
            {
                propertyType = PropertyTypeHelpers.ToPropertyType(category);
            }

            return propertyType;
        }

        private static SalePricing ExtractSalePricing(XElement document, CultureInfo cultureInfo)
        {
            document.ShouldNotBe(null);

            var salePricing = new SalePricing
            {
                SalePrice = document.MoneyValueOrDefault(cultureInfo, "price")
            };

            // Selling data.

            var salePriceText = document.ValueOrDefault("priceView");
            var displayAttributeValue = document.ValueOrDefault("priceView", "display");
            var isDisplay = string.IsNullOrWhiteSpace(displayAttributeValue) ||
                            displayAttributeValue.ParseOneYesZeroNoToBool();
            salePricing.SalePriceText = isDisplay
                ? salePriceText
                : string.IsNullOrWhiteSpace(salePriceText)
                    ? "Price Witheld"
                    : salePriceText;

            var isUnderOffer = document.ValueOrDefault("underOffer", "value");
            salePricing.IsUnderOffer = !string.IsNullOrWhiteSpace(isUnderOffer) &&
                                       isUnderOffer.ParseOneYesZeroNoToBool();


            // Sold data.
            var soldDetails = document.Element("soldDetails");
            if (soldDetails != null)
            {
                // SoldPrice could be price or soldPrice. Thanks REA for such a great schema.
                var soldPrice = soldDetails.Element("price") ??
                                soldDetails.Element("soldPrice");
                if (soldPrice != null)
                {
                    ExtractSoldPrice(soldPrice, salePricing);
                }

                var soldDate = soldDetails.Element("date") ??
                               soldDetails.Element("soldDate");
                if (soldDate != null)
                {
                    ExtractSoldOn(soldDate, salePricing);
                }
            }

            return salePricing;
        }

        private static void ExtractSoldPrice(XElement element, SalePricing salePricing)
        {
            element.ShouldNotBe(null);

            salePricing.SoldPrice = element.DecimalValueOrDefault();

            var soldDisplayAttribute = element.ValueOrDefault(null, "display");
            // NOTE: no display price assumes a 'YES' and that the price -is- to be displayed.
            salePricing.SoldPriceText = string.IsNullOrWhiteSpace(soldDisplayAttribute) ||
                                        soldDisplayAttribute.ParseOneYesZeroNoToBool()
                ? null
                : "Sold Price Witheld";
        }

        private static void ExtractSoldOn(XElement element, SalePricing salePricing)
        {
            element.ShouldNotBe(null);

            // SoldOn could be date or soldData. Thanks REA for such a great schema.
            var soldOnText = element.ValueOrDefault();
            if (!string.IsNullOrWhiteSpace(soldOnText))
            {
                salePricing.SoldOn = ToDateTime(soldOnText);
            }
        }

        private static DateTime? ExtractAuction(XElement document)
        {
            document.ShouldNotBe(null);

            var auction = document.ValueOrDefault("auction");

            // NOTE: The REA documentation is vague as to the 100% specification on this.
            // So i'm going to assume the following (in order)
            // 1. <auction>date-time-in-here</auction>
            // 2. <auction date="date-time-in-here"></auction>
            // ** YET ANOTHER FRICKING EXAMPLE OF WHY THIS SCHEMA AND XML ARE F'ING CRAP **
            if (string.IsNullOrWhiteSpace(auction))
            {
                auction = document.ValueOrDefault("auction", "date");
            }

            return (!string.IsNullOrWhiteSpace(auction))
                ? ToDateTime(auction)
                : null;
        }

        private static LandDetails ExtractLandDetails(XElement document)
        {
            document.ShouldNotBe(null);

            var landDetailsElement = document.Element("landDetails");
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

            var details = new LandDetails
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
                    Value = depthValue,
                    Type = string.IsNullOrWhiteSpace(depthType)
                        ? "Total"
                        : depthType,
                    Side = depthSide
                };
            }

            return details;
        }

        /// <summary>
        /// REA Specific DateTime parsing.
        /// </summary>
        private static DateTime ParseReaDateTime(string someDateTime)
        {
            someDateTime.ShouldNotBeNullOrEmpty();

            // FFS REA!!!! URGH!!!!!!!! :( 
            // Stick to fricking ISO8061 with yyyy-MM-ddTHH:mm:ss 
            // ONE FORMAT TO RULE THEM ALL.
            // (not that hard, peeps).
            var reaDateTimeFormats = new[]
            {
                "yyyy-MM-dd",
                "yyyy-MM-dd-HH:mm",
                "yyyy-MM-dd-HH:mm:ss",
                "yyyy-MM-ddTHH:mm",
                "yyyy-MM-ddTHH:mm:ss",
                "yyyyMMdd",
                "yyyyMMdd-HHmm",
                "yyyyMMDD-HHmmss",
                "yyyyMMDDTHHmm",
                "yyyyMMddTHHmmss"
            };

            DateTime resultDateTime;

            if (!DateTime.TryParse(someDateTime, out resultDateTime))
            {
                DateTime.TryParseExact(someDateTime.Trim(), reaDateTimeFormats, CultureInfo.InvariantCulture,
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

        private static void ExtractResidentialData(ResidentialListing residentialListing, XElement xElement, CultureInfo cultureInfo)
        {
            residentialListing.ShouldNotBe(null);
            xElement.ShouldNotBe(null);

            residentialListing.PropertyType = ExtractResidentialAndRentalPropertyType(xElement);
            residentialListing.Pricing = ExtractSalePricing(xElement, cultureInfo);
            residentialListing.AuctionOn = ExtractAuction(xElement);
            residentialListing.Features = ExtractFeatures(xElement);
        }

        #endregion

        #region Rental Listing Methods

        private static void ExtractRentalData(RentalListing rentalListing, XElement xElement, CultureInfo cultureInfo)
        {
            rentalListing.ShouldNotBe(null);
            xElement.ShouldNotBe(null);

            var dateAvailble = xElement.ValueOrDefault("dateAvailable");
            if (!string.IsNullOrWhiteSpace(dateAvailble))
            {
                rentalListing.AvailableOn = ToDateTime(dateAvailble);
            }

            rentalListing.PropertyType = ExtractResidentialAndRentalPropertyType(xElement);
            rentalListing.Pricing = ExtractRentalPricing(xElement, cultureInfo);
            rentalListing.Features = ExtractFeatures(xElement);
        }

        // REF: http://reaxml.realestate.com.au/docs/reaxml1-xml-format.html#rent
        private static RentalPricing ExtractRentalPricing(XElement xElement, CultureInfo cultureInfo)
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
            foreach (var rentElement in rentElements)
            {
                // Have to have a period.
                var frequency = rentElement.AttributeValueOrDefault("period");
                if (string.IsNullOrWhiteSpace(frequency))
                {
                    continue;
                }

                if (frequency.Equals("week", StringComparison.InvariantCultureIgnoreCase) ||
                    frequency.Equals("weekly", StringComparison.InvariantCultureIgnoreCase))
                {
                    rentalPricing.PaymentFrequencyType = PaymentFrequencyType.Weekly;
                }
                else if (frequency.Equals("month", StringComparison.InvariantCultureIgnoreCase) ||
                    frequency.Equals("monthly", StringComparison.InvariantCultureIgnoreCase))
                {
                    rentalPricing.PaymentFrequencyType = PaymentFrequencyType.Monthly;
                }

                rentalPricing.RentalPrice = rentElement.MoneyValueOrDefault(cultureInfo);

                var displayAttributeValue = rentElement.AttributeValueOrDefault("display");
                var isDisplay = string.IsNullOrWhiteSpace(displayAttributeValue) ||
                                displayAttributeValue.ParseOneYesZeroNoToBool();
                rentalPricing.RentalPriceText = isDisplay
                    ? rentalPricing.RentalPrice.ToString()
                    : null;

                // NOTE: We only parse the first one. You have more than one? Pffftttt!!! Die!
                break;
            }

            // NOTE: Even though we have set the rental price text to be the last
            //       rental period's value ... this can now be overwritten by
            //       whatever value they might have in here ... if they have a value.
            var priceView = xElement.ValueOrDefault("priceView");
            if (!string.IsNullOrWhiteSpace(priceView))
            {
                rentalPricing.RentalPriceText = priceView;
            } 

            rentalPricing.Bond = xElement.MoneyValueOrDefault(cultureInfo, "bond");

            return rentalPricing;
        }

        #endregion

        #region Land Listing Methods

        private static void ExtractLandData(LandListing landListing, XElement xElement, CultureInfo cultureInfo)
        {
            landListing.ShouldNotBe(null);
            xElement.ShouldNotBe(null);

            landListing.CategoryType = ExtractLandCategoryType(xElement);
            landListing.Pricing = ExtractSalePricing(xElement, cultureInfo);
            landListing.AuctionOn = ExtractAuction(xElement);
            landListing.Estate = ExtractLandEstate(xElement);
            landListing.AuctionOn = ExtractAuction(xElement);
        }

        private static Core.Models.Land.CategoryType ExtractLandCategoryType(XElement xElement)
        {
            var category = xElement.ValueOrDefault("landCategory", "name");
            return string.IsNullOrWhiteSpace(category)
                ? Core.Models.Land.CategoryType.Unknown
                : CategoryTypeHelpers.ToCategoryType(category);
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

        #endregion

        #region Rural Listing Methods

        private static void ExtractRuralData(RuralListing listing, XElement document, CultureInfo cultureInfo)
        {
            document.ShouldNotBe(null);

            listing.CategoryType = ExtractRuralCategoryType(document);
            listing.Pricing = ExtractSalePricing(document, cultureInfo);
            listing.AuctionOn = ExtractAuction(document);
        }

        private static Core.Models.Rural.CategoryType ExtractRuralCategoryType(XElement document)
        {
            document.ShouldNotBe(null);

            var categoryElement = document.Element("ruralCategory");
            if (categoryElement == null)
            {
                return Core.Models.Rural.CategoryType.Unknown;
            }

            var categoryValue = categoryElement.AttributeValueOrDefault("name");
            return string.IsNullOrWhiteSpace(categoryValue)
                ? Core.Models.Rural.CategoryType.Unknown
                : Core.Models.Rural.CategoryTypeHelpers.ToCategoryType(categoryValue);
        }

        #endregion

        private class SplitElementResult
        {
            public IList<XElement> KnownXmlData { get; set; }
            public IList<XElement> UnknownXmlData { get; set; }
        }
    }
}