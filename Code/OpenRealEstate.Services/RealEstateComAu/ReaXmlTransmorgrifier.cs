using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

        public ReaXmlTransmorgrifier()
        {
            AddressDelimeter = "/";
        }

        /// <summary>
        /// Converts some given data into a listing instance.
        /// </summary>
        /// <param name="data">some data source, like Xml data or json data.</param>
        /// <param name="areBadCharactersRemoved">Help clean up the data.</param>
        /// <param name="isClearAllIsModified">After the data is loaded, do we clear all IsModified fields so it looks like the listing(s) are all ready to be used and/or compared against other listings.</param>
        /// /// <returns>List of listings and any unhandled data.</returns>
        public ConvertToResult ConvertTo(string data, 
            bool areBadCharactersRemoved = false,
            bool isClearAllIsModified = false)
        {
            data.ShouldNotBeNullOrEmpty();

            // Remove any BOM if one exists.
            // REF: http://stackoverflow.com/questions/5098757/how-to-tell-asciiencoding-class-not-to-decode-the-byte-order-mark
            data = RemoveByteOrderMark(data);

            var validationErrorMessage = ValidateXmlString(data);
            if (!string.IsNullOrWhiteSpace(validationErrorMessage))
            {
                if (!areBadCharactersRemoved)
                {
                    return new ConvertToResult
                    {
                        Errors = new List<ParsedError>
                    {
                        new ParsedError(validationErrorMessage, "The entire data source.")}
                    };
                }

                // Some bad data occurs, so lets clean any bad data out.
                data = RemoveInvalidXmlChars(data);
            }

            // Now split it up into the known listing types.
            SplitElementResult elements;

            try
            {
                elements = SplitReaXmlIntoElements(data);
            }
            catch (Exception exception)
            {
                return new ConvertToResult
                {
                    Errors = new List<ParsedError>
                    {
                        new ParsedError(exception.Message, 
                            "Failed to parse the provided xml data because it contains some invalid data. Pro Tip: This is usually because a character is not encoded. Like an ampersand.")
                    }
                };
            }
            
            if (!elements.KnownXmlData.Any() &&
                !elements.UnknownXmlData.Any())
            {
                return null;
            }

            // Finally, we convert each segment into a listing.
            var successfullyParsedListings = new ConcurrentBag<ListingResult>();
            var invalidData = new ConcurrentBag<ParsedError>();

            Parallel.ForEach(elements.KnownXmlData, element =>
            {
                try
                {
                    successfullyParsedListings.Add(new ListingResult
                    {
                        Listing = ConvertFromReaXml(element, DefaultCultureInfo, AddressDelimeter, isClearAllIsModified),
                        SourceData = element.ToString()
                    });
                }
                catch (Exception exception)
                {
                    invalidData.Add(new ParsedError(exception.Message, element.ToString()));
                }
            });

            return new ConvertToResult
            {
                Listings = successfullyParsedListings.Any()
                    ? successfullyParsedListings.ToList()
                    : null,
                UnhandledData = elements.UnknownXmlData != null &&
                                elements.UnknownXmlData.Any()
                    ? elements.UnknownXmlData.Select(x => x.ToString()).ToList()
                    : null,
                Errors = invalidData.Any()
                    ? invalidData.ToList()
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

        public string AddressDelimeter { get; set; }

        private static string RemoveByteOrderMark(string text)
        {
            var byteOrderMark = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble()).ToCharArray();

            var startOfTextChars = text.Substring(0, byteOrderMark.Length).ToCharArray();
            for (int i = byteOrderMark.Length - 1; i >= 0; i--)
            {
                if (startOfTextChars[i] == byteOrderMark[i])
                {
                    text = text.Remove(i, 1);
                }
            }

            return text;
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

            // If there are bad elements in the XML, then this throw an exception.
            // Eg. & (ampersands) in video links that are not properly encoded, etc.
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

        private static Listing ConvertFromReaXml(XElement document, 
            CultureInfo cultureInfo,
            string addressDelimeter,
            bool isClearAllIsModified)
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
            ExtractCommonData(listing, document, addressDelimeter);

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

            if (isClearAllIsModified)
            {
                listing.ClearAllIsModified();
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

        private static void ExtractCommonData(Listing listing, 
            XElement document, 
            string addressDelimeter)
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

            listing.Address = ExtractAddress(document, addressDelimeter);
            
            var agents = ExtractAgent(document);
            if (agents != null &&
                agents.Any())
            {
                listing.AddAgents(agents);
            }

            var inspections = ExtractInspectionTimes(document);
            if (inspections != null &&
                inspections.Any())
            {
                listing.AddInspections(inspections);
            }

            var images = ExtractImages(document);
            if (images != null &&
                images.Any())
            {
                listing.AddImages(images);
            }
            
            var floorPlans = ExtractFloorPlans(document);
            if (floorPlans != null &&
                floorPlans.Any())
            {
                listing.AddFloorPlans(floorPlans);
            }
            
            var videos = ExtractVideos(document);
            if (videos != null 
                && videos.Any())
            {
                listing.AddVideos(videos);
            }
            listing.Features = ExtractFeatures(document);
            listing.LandDetails = ExtractLandDetails(document);

            var links = ExtractExternalLinks(document);
            if (links != null &&
                links.Any())
            {
                listing.AddLinks(links);
            }
        }

        private static Address ExtractAddress(XElement document, string addressDelimeter)
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
            var streetNumber = addressElement.ValueOrDefault("streetNumber");

            // LOGIC:
            // So, we're trying to create a streetnumber value that contains the rea values
            //     Sub Number
            //     Lot Number
            //     Street Number
            // into a single value. URGH.
            // This is because REA have over fricking complicated shiz (again). So lets just
            // keep this simple, eh? :)
            
            // FORMAT: subnumber lotnumber streetnumber
            // eg. 23a/135 smith street
            //     6/23a 135 smith street
            //     23a lot 33 smith street
            //     23a lot 33/135 smith street

            // Lot number logic: If the value contains the word LOT in it, then we don't
            // need to do anything. Otherwise, we should have a value the starts with 'LOT'.
            // eg. LOT 123abc
            var lotNumberResult = string.IsNullOrWhiteSpace(lotNumber)
                ? string.Empty
                : lotNumber.IndexOf("lot", StringComparison.InvariantCultureIgnoreCase) > 0
                    ? lotNumber
                    : string.Format("LOT {0}", lotNumber);

            // Sub number and Street number logic: A sub number can exist -before- the street number.
            // A street number might NOT exist, so a sub number is all by itself.
            // When we want to show a sub number, we probably want to show it, like this:
            //    'subNumber`delimeter`streetNumber`
            //   eg. 12a/432
            // But .. sometimes, the sub number -already- contains a delimeter! so then we want this:
            //   eg. 45f/231 15
            // So we don't put a delimeter in there, but a space. Urgh! confusing, so sowwy.

            var subNumberLotNumber = string.Format("{0} {1}",
                subNumber,
                lotNumberResult).Trim();

            // We only have a delimeter if we have a sub-or-lot number **and**
            // a street number.
            // Also, we use the default delimeter if we don't have one already in the
            // sub-or-lot number.
            var delimeter = string.IsNullOrWhiteSpace(subNumberLotNumber)
                ? string.Empty
                : subNumberLotNumber.IndexOfAny(new[] {'/', '\\', '-'}) > 0
                    ? " "
                    : string.IsNullOrWhiteSpace(streetNumber)
                        ? string.Empty
                        : addressDelimeter;

            address.StreetNumber = string.Format("{0}{1}{2}",
                subNumberLotNumber,
                delimeter,
                streetNumber).Trim();

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


            // Technically, the <municipality/> element is not a child of the <address/> element.
            // But I feel that it's sensible to still parse for it, in here.
            address.Municipality = document.ValueOrDefault("municipality");

            // Finally - Lat/Longs. These are -not- part of the REA XML standard.
            // ~BUT~ some multi-loaders are sticking this data into some xml!
            ExtractLatitudeLongitudes(document, address);

            return address;
        }

        private static void ExtractLatitudeLongitudes(XElement document, Address address)
        {
            document.ShouldNotBe(null);
            address.ShouldNotBe(null);

            var latitudeElement = document.Descendants("Latitude").FirstOrDefault() ??
                                  document.Descendants("latitude").FirstOrDefault();
            if (latitudeElement != null)
            {
                address.Latitude = latitudeElement.DecimalValueOrDefault();
            }

            var longitudeElement = document.Descendants("Longitude").FirstOrDefault() ??
                                   document.Descendants("longitude").FirstOrDefault();
            if (latitudeElement != null)
            {
                address.Longitude = longitudeElement.DecimalValueOrDefault();
            }
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
                var name = agentElement.ValueOrDefault("name");
                if (string.IsNullOrWhiteSpace(name))
                {
                    // We need a name of the agent at the very least.
                    // Some listings have this element but no data provided. :(
                    // So we don't add 'emtpy' agents.
                    continue;
                }

                var agent = new ListingAgent
                {
                    Name = name
                };

                var orderValue = agentElement.AttributeValueOrDefault("id");
                int order = 0;
                if (!string.IsNullOrWhiteSpace(orderValue) &&
                    int.TryParse(orderValue, out order))
                {
                    agent.Order = order;
                }

                var communications = new List<Communication>();

                var email = agentElement.ValueOrDefault("email");
                if (!string.IsNullOrWhiteSpace(email))
                {
                    communications.Add(new Communication
                    {
                        CommunicationType = CommunicationType.Email,
                        Details = email
                    });
                }
                
                var phoneMobile = agentElement.ValueOrDefault("telephone", "type", "mobile");
                if (!string.IsNullOrWhiteSpace(phoneMobile))
                {
                    communications.Add(new Communication
                    {
                        CommunicationType = CommunicationType.Mobile,
                        Details = phoneMobile
                    });
                }

                var phoneOffice = agentElement.ValueOrDefault("telephone", "type", "BH");
                if (!string.IsNullOrWhiteSpace(phoneOffice))
                {
                    communications.Add(new Communication
                    {
                        CommunicationType = CommunicationType.Landline,
                        Details = phoneOffice
                    });
                }

                if (communications.Any())
                {
                    agent.AddCommunications(communications);
                }

                // Don't add this agent, if the name already exists in the list.
                if (!agents.Any(x => x.Name.Equals(agent.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    // This agent doesn't exists - so we're good to add them!
                    agents.Add(agent);
                }
            }

            var counter = 0;
            if (agents.Any())
            {
                var orderedAgents = new List<ListingAgent>();
                foreach (var agent in agents.OrderBy(x => x.Order))
                {
                    var orderedAgent = new ListingAgent
                    {
                        Name = agent.Name,
                        Order = ++counter
                    };
                    if (agent.Communications.Any())
                    {
                        orderedAgent.AddCommunications(agent.Communications);
                    }
                    orderedAgents.Add(orderedAgent);
                }

                return orderedAgents;
            }

            return null;
        }

        private static Features ExtractFeatures(XElement document)
        {
            document.ShouldNotBe(null);

            var featuresElement = document.Element("features");
            if (featuresElement == null)
            {
                return null;
            }

            var tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // NOTE: Bedrooms can be a number -or- the value 'STUDIO'.
            //       YES - where a number is the logical value, they can now have a string. :cry:
            //       So, if the value is a string, like STUDIO (or anything else), then the
            //       value will be returned as ZERO.
            //       If it's a STUDIO, we'll add that to the feature's tag hash set.
            var bedroomsValue = featuresElement.ValueOrDefault("bedrooms");
            var bedrooms = 0;
            if (!string.IsNullOrWhiteSpace(bedroomsValue))
            {
                if (bedroomsValue.Equals("studio", StringComparison.OrdinalIgnoreCase))
                {
                    // *epic le sigh - yes, we have a text value for (what looks like) a number value.
                    tags.Add("bedroom-studio");
                }
                else
                {
                    bedrooms = featuresElement.ByteValueOrDefault("bedrooms");
                }
            }
            
            ExtractFeatureWithTextValues(featuresElement,
                "heating",
                new[] {"gas", "electric", "GDH", "solid", "other"},
                tags);

            ExtractFeatureWithTextValues(featuresElement,
                "hotWaterService",
                new[] {"gas", "electric", "solar"},
                tags);

            ExtractFeatureWithTextValues(featuresElement,
                "pool",
                new[] { "inground", "aboveground" },
                tags,
                null);

            ExtractFeatureWithTextValues(featuresElement,
                "spa",
                new[] { "inground", "aboveground" },
                tags,
                null);

            ExtractOtherFeatures(featuresElement, tags);

            // Now for the final, tricky part - extracting all the boolean stuff into tags.
            foreach (var feature in new[] {"features", "allowances", "ecoFriendly"}
                .Select(node => document.Element(node))
                .Where(element => element != null).Select(ExtractBooleanFeatures)
                .Where(features => features.Any()).SelectMany(features => features))
            {
                tags.Add(feature);
            }

            var finalFeatures = new Features
            {
                Bedrooms = bedrooms,
                Bathrooms = featuresElement.ByteValueOrDefault("bathrooms"),
                CarParking = new CarParking
                {
                    Garages = featuresElement.BoolOrByteValueOrDefault("garages"),
                    Carports = featuresElement.BoolOrByteValueOrDefault("carports"),
                    OpenSpaces = featuresElement.BoolOrByteValueOrDefault("openSpaces")
                },
                Ensuites = featuresElement.BoolOrByteValueOrDefault("ensuite"),
                Toilets = featuresElement.BoolOrByteValueOrDefault("toilets"),
                LivingAreas = featuresElement.BoolOrByteValueOrDefault("livingAreas"),
            };
            if (tags.Any())
            {
                finalFeatures.AddTags(tags);
            }

            return finalFeatures;
        }

        private static void ExtractFeatureWithTextValues(XElement document, 
            string elementName, 
            string[] validValues,
            ISet<string> tags,
            string delimeter = "-")
        {
            document.ShouldNotBe(null);
            elementName.ShouldNotBeNullOrEmpty();
            validValues.ShouldNotBe(null);
            tags.ShouldNotBe(null);

            var type = document.ValueOrDefault(elementName, ("type"));
            if (string.IsNullOrWhiteSpace(type))
            {
                return;
            }

            if (validValues.Contains(type, StringComparer.InvariantCultureIgnoreCase))
            {
                tags.Add(string.Format("{0}{1}{2}",
                    elementName,
                    string.IsNullOrWhiteSpace(delimeter)
                        ? string.Empty
                        : delimeter,
                    type));
            }
        }

        private static void ExtractOtherFeatures(XElement document, ISet<string> tags)
        {
            document.ShouldNotBe(null);
            tags.ShouldNotBe(null);

            var value = document.ValueOrDefault("otherFeatures");
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            // Split the value up into comma delimeted parts.
            var parts = value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                tags.Add(part.Trim());
            }
        }

        private static ISet<string> ExtractBooleanFeatures(XElement document)
        {
            var suppliedFeatures = new ConcurrentBag<string>();

            Parallel.ForEach(XmlFeatureHelpers.OtherFeatureNames, possibleFeature =>
            {
                if (document.BoolValueOrDefault(possibleFeature))
                {
                    suppliedFeatures.Add(possibleFeature);
                }
            });

            return new HashSet<string>(suppliedFeatures);
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

            // Images can go in either <images> or <objects>. Only idiots are putting them in <objects>
            // Generally, <objects> is reservered for floorplans. :/
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

            return ConvertMediaXmlDataToMedia(imagesElements, ConvertImageOrderToNumber);
        }

        private static IList<Media> ExtractFloorPlans(XElement document)
        {
            document.ShouldNotBe(null);

            // NOTE: The idea is that <images> will contain images while <objects> will only be for floorplans.
            //       Yes, some listings put their images in <objects>, but this is handled elsewhere.
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

            return ConvertMediaXmlDataToMedia(floorPlanElements, Convert.ToInt32);
        }

        private static IList<Media> ConvertMediaXmlDataToMedia(IEnumerable<XElement> mediaElements, 
            Func<string, int> orderConverstionFunction)
        {
            // Note: Image 'urls' can either be via a Uri (yay!) or
            //       a file name because the xml was provided in a zip file with
            //       the images (booooo! hiss!!!)
            var media = (from x in mediaElements
                          let url = x.AttributeValueOrDefault("url")
                          let file = x.AttributeValueOrDefault("file")
                          let order = x.AttributeValueOrDefault("id")
                          let createdOn = x.AttributeValueOrDefault("modTime")
                          where (!string.IsNullOrWhiteSpace(url) ||
                                 !string.IsNullOrWhiteSpace(file)) &&
                                !string.IsNullOrWhiteSpace(order) &&
                                !string.IsNullOrWhiteSpace(createdOn)
                          select new Media
                          {
                              CreatedOn = ParseReaDateTime(createdOn),
                              Url = string.IsNullOrWhiteSpace(url)
                                  ? string.IsNullOrWhiteSpace(file)
                                      ? null
                                      : file
                                  : url,
                              Order = orderConverstionFunction(order)
                          }).ToList();

            return media.Any()
                ? media
                : null;
        }

        private static IList<Media> ExtractVideos(XElement document)
        {
            document.ShouldNotBe(null);

            var videoUrl = document.ValueOrDefault("videoLink", "href");
            return string.IsNullOrWhiteSpace(videoUrl)
                ? null
                : new List<Media>
                {
                    new Media
                    {
                        Order = 1,
                        Url = videoUrl
                    }
                };
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

            var salePricing = new SalePricing();

            ExtractSalePrice(document, salePricing, cultureInfo);
            
            ExtractSoldDetails(document, salePricing);

            return salePricing;
        }

        /* Eg xml.
           <residential ...
             <price display="yes">500000</price>
             <priceView>Between $400,000 and $600,000</priceView>
           />
        */
        private static void ExtractSalePrice(XElement document, 
            SalePricing salePricing,
            CultureInfo cultureInfo)
        {
            document.ShouldNotBe(null);
            salePricing.ShouldNotBe(null);

            salePricing.SalePrice = document.MoneyValueOrDefault(cultureInfo, "price");

            var salePriceText = document.ValueOrDefault("priceView");
            var displayAttributeValue = document.ValueOrDefault("price", "display");
            var isDisplay = string.IsNullOrWhiteSpace(displayAttributeValue) ||
                            displayAttributeValue.ParseOneYesZeroNoToBool();
            var doesSalePriceExists = !string.IsNullOrWhiteSpace(document.ValueOrDefault("price"));

            // NOTE 1: If Display="no" then we do not display anything for the price, regardless
            //       of any other data provided. Otherwise, make a decision.
            // NOTE 2: If -NO- saleprice is provided (eg. this is _very_ common when we get
            //         an SOLD or LEASED, etc) then we should leave the sale price text alone.
            //         So only do the sale-price-text checks if we have a value set AND
            //         it's ok to display a value.
            // NOTE 3: display='no' means NO price is displayed, even if there's a priceText.
            salePricing.SalePriceText = isDisplay &&
                                        doesSalePriceExists
                ? string.IsNullOrWhiteSpace(salePriceText)
                    ? salePricing.SalePrice.ToString("C0")
                    : salePriceText
                : null;

            var isUnderOffer = document.ValueOrDefault("underOffer", "value");
            salePricing.IsUnderOffer = !string.IsNullOrWhiteSpace(isUnderOffer) &&
                                       isUnderOffer.ParseOneYesZeroNoToBool();
        }

        /* Eg xml.
           <residential ...
             <soldDetails>
               <price display="yes">580000</price>
               <date>2009-01-10-12:30:00</date>
             </soldDetails>
           />
        */
        private static void ExtractSoldDetails(XElement document, SalePricing salePricing)
        {
            document.ShouldNotBe(null);
            salePricing.ShouldNotBe(null);

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
        }

        // Eg xml: <price display="yes">580000</price>
        private static void ExtractSoldPrice(XElement document, SalePricing salePricing)
        {
            document.ShouldNotBe(null);
            salePricing.ShouldNotBe(null);

            salePricing.SoldPrice = document.DecimalValueOrDefault();

            // NOTE 1: no display price assumes a 'YES' and that the price -is- to be displayed.
            // NOTE 2: A _display attribute_ value of 'range' can only valid for commerical properties ...
            //         and .. we don't handle commerical. So it will end up throwing an exception
            //         which is legit in this case.
            // NOTE 3: display='no' means NO price is displayed, even if there's a priceText.
            var soldDisplayAttribute = document.ValueOrDefault(null, "display");
            var isDisplay = string.IsNullOrWhiteSpace(soldDisplayAttribute) ||
                            soldDisplayAttribute.ParseOneYesZeroNoToBool();

            salePricing.SoldPriceText = isDisplay &&
                                        salePricing.SoldPrice > 0
                ? string.IsNullOrWhiteSpace(salePricing.SoldPriceText)
                    ? salePricing.SoldPrice.Value.ToString("C0")
                    : salePricing.SoldPriceText
                : null;
        }

        // Eg xml: <date>2009-01-10-12:30:00</date>
        private static void ExtractSoldOn(XElement document, SalePricing salePricing)
        {
            document.ShouldNotBe(null);

            // SoldOn could be date or soldData. Thanks REA for such a great schema.
            var soldOnText = document.ValueOrDefault();
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

        private static BuildingDetails ExtractBuildingDetails(XElement document)
        {
            document.ShouldNotBe(null);

            var buildingDetailsElement = document.Element("buildingDetails");
            if (buildingDetailsElement == null)
            {
                return null;
            }

            var energyRating = buildingDetailsElement.DecimalValueOrDefault("energyRating");

            return new BuildingDetails
            {
                Area = buildingDetailsElement.UnitOfMeasureOrDefault("area", "unit"),
                EnergyRating = energyRating <= 0
                    ? (decimal?) null
                    : energyRating,
            };
        }

        private static LandDetails ExtractLandDetails(XElement document)
        {
            document.ShouldNotBe(null);

            var landDetailsElement = document.Element("landDetails");
            if (landDetailsElement == null)
            {
                return null;
            }

            var details = new LandDetails
            {
                Area = landDetailsElement.UnitOfMeasureOrDefault("area", "unit"),
                Frontage = landDetailsElement.UnitOfMeasureOrDefault("frontage", "unit"),
                CrossOver = landDetailsElement.ValueOrDefault("crossOver", "value")
            };

            var depthElements = landDetailsElement.Elements("depth").ToArray();
            if (depthElements.Any())
            {
                foreach (var depthElement in depthElements)
                {
                    var depthValue = depthElement.DecimalValueOrDefault();
                    var depthType = depthElement.AttributeValueOrDefault("unit");
                    var depthSide = depthElement.AttributeValueOrDefault("side");

                    if (depthValue > 0)
                    {
                        var depth = new Depth
                        {
                            Value = depthValue,
                            Type = string.IsNullOrWhiteSpace(depthType)
                                ? "Total"
                                : depthType,
                            Side = depthSide
                        };

                        details.AddDepths(new[] {depth});
                    }
                }
            }
            
            return details;
        }

        private static IList<string> ExtractExternalLinks(XElement document)
        {
            document.ShouldNotBe(null);

            var elements = document.Elements("externalLink").ToArray();
            if (!elements.Any())
            {
                return null;
            }

            return (from e in elements
                let externalLink = e.AttributeValueOrDefault("href")
                where !string.IsNullOrWhiteSpace(externalLink)
                select Uri.UnescapeDataString(externalLink.Trim()))
                .ToList();
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

        private static void ExtractResidentialData(ResidentialListing residentialListing, 
            XElement document, 
            CultureInfo cultureInfo)
        {
            residentialListing.ShouldNotBe(null);
            document.ShouldNotBe(null);

            residentialListing.PropertyType = ExtractResidentialAndRentalPropertyType(document);
            residentialListing.Pricing = ExtractSalePricing(document, cultureInfo);
            residentialListing.AuctionOn = ExtractAuction(document);
            residentialListing.BuildingDetails = ExtractBuildingDetails(document);
            residentialListing.CouncilRates = document.ValueOrDefault("councilRates");
            ExtractHomeAndLandPackage(document, residentialListing);
            ExtractResidentialNewConstruction(document, residentialListing);
        }

        private static void ExtractHomeAndLandPackage(XElement document, ResidentialListing residentialListing)
        {
            document.ShouldNotBe(null);
            residentialListing.ShouldNotBe(null);

            var homeAndLandPackageElement = document.Element("isHomeLandPackage");
            if (homeAndLandPackageElement == null)
            {
                return;
            }

            if (homeAndLandPackageElement.AttributeBoolValueOrDefault("value"))
            {
                if (residentialListing.Features == null)
                {
                    residentialListing.Features = new Features();
                }

                residentialListing.Features.AddTags(new[] {"houseAndLandPackage"});
            };

        }

        private static void ExtractResidentialNewConstruction(XElement document, ResidentialListing listing)
        {
            document.ShouldNotBe(null);
            listing.ShouldNotBe(null);

            if (!document.BoolValueOrDefault("newConstruction"))
            {
                return;
            }

            if (listing.Features == null)
            {
                listing.Features= new Features();
            }

            listing.Features.AddTags(new[] {"isANewConstruction"});
        }

        #endregion

        #region Rental Listing Methods

        private static void ExtractRentalData(RentalListing rentalListing, XElement document, CultureInfo cultureInfo)
        {
            rentalListing.ShouldNotBe(null);
            document.ShouldNotBe(null);

            var dateAvailble = document.ValueOrDefault("dateAvailable");
            if (!string.IsNullOrWhiteSpace(dateAvailble))
            {
                rentalListing.AvailableOn = ToDateTime(dateAvailble);
            }

            rentalListing.PropertyType = ExtractResidentialAndRentalPropertyType(document);
            rentalListing.Pricing = ExtractRentalPricing(document, cultureInfo);
            rentalListing.Features = ExtractFeatures(document);
            rentalListing.BuildingDetails = ExtractBuildingDetails(document);
            ExtractRentalNewConstruction(document, rentalListing);
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
                    ? rentalPricing.RentalPrice.ToString("C0")
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

            rentalPricing.Bond = xElement.NullableMoneyValueOrDefault(cultureInfo, "bond");

            return rentalPricing;
        }

        private static void ExtractRentalNewConstruction(XElement document, RentalListing listing)
        {
            document.ShouldNotBe(null);
            listing.ShouldNotBe(null);

            if (!document.BoolValueOrDefault("newConstruction"))
            {
                return;
            }

            if (listing.Features == null)
            {
                listing.Features = new Features();
            }

            listing.Features.AddTags(new[] {"isANewConstruction"});
        }

        #endregion

        #region Land Listing Methods

        private static void ExtractLandData(LandListing landListing, XElement document, CultureInfo cultureInfo)
        {
            landListing.ShouldNotBe(null);
            document.ShouldNotBe(null);

            landListing.CategoryType = ExtractLandCategoryType(document);
            landListing.Pricing = ExtractSalePricing(document, cultureInfo);
            landListing.AuctionOn = ExtractAuction(document);
            landListing.Estate = ExtractLandEstate(document);
            landListing.AuctionOn = ExtractAuction(document);
            landListing.CouncilRates = document.ValueOrDefault("councilRates");
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

        private static void ExtractRuralData(RuralListing ruralListing, XElement document, CultureInfo cultureInfo)
        {
            document.ShouldNotBe(null);

            ruralListing.CategoryType = ExtractRuralCategoryType(document);
            ruralListing.Pricing = ExtractSalePricing(document, cultureInfo);
            ruralListing.AuctionOn = ExtractAuction(document);
            ruralListing.RuralFeatures = ExtractRuralFeatures(document);
            ruralListing.CouncilRates = ExtractRuralCouncilRates(document);
            ruralListing.BuildingDetails = ExtractBuildingDetails(document);
            ExtractRuralNewConstruction(document, ruralListing);
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

        private static RuralFeatures ExtractRuralFeatures(XElement document)
        {
            document.ShouldNotBe(null);
            var ruralFeaturesElement = document.Element("ruralFeatures");
            if (ruralFeaturesElement == null)
            {
                return null;
            }

            return new RuralFeatures
            {
                AnnualRainfall = ruralFeaturesElement.ValueOrDefault("annualRainfall"),
                CarryingCapacity = ruralFeaturesElement.ValueOrDefault("carryingCapacity"),
                Fencing = ruralFeaturesElement.ValueOrDefault("fencing"),
                Improvements = ruralFeaturesElement.ValueOrDefault("improvements"),
                Irrigation = ruralFeaturesElement.ValueOrDefault("irrigation"),
                Services = ruralFeaturesElement.ValueOrDefault("services"),
                SoilTypes = ruralFeaturesElement.ValueOrDefault("soilTypes")
            };
        }

        private static string ExtractRuralCouncilRates(XElement document)
        {
            document.ShouldNotBe(null);
            var ruralFeaturesElement = document.Element("ruralFeatures");
            return ruralFeaturesElement == null
                ? null
                : ruralFeaturesElement.ValueOrDefault("councilRates");
        }

        private static void ExtractRuralNewConstruction(XElement document, RuralListing listing)
        {
            document.ShouldNotBe(null);
            listing.ShouldNotBe(null);

            if (!document.BoolValueOrDefault("newConstruction"))
            {
                return;
            }

            if (listing.Features == null)
            {
                listing.Features = new Features();
            }

            listing.Features.AddTags(new[] {"isANewConstruction"});
        }

        #endregion

        private class SplitElementResult
        {
            public IList<XElement> KnownXmlData { get; set; }
            public IList<XElement> UnknownXmlData { get; set; }
        }
    }
}