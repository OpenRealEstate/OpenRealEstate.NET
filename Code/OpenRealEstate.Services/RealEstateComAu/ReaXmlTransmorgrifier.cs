using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Land;
using OpenRealEstate.Core.Rental;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Core.Rural;
using CategoryTypeHelpers = OpenRealEstate.Core.Land.CategoryTypeHelpers;

namespace OpenRealEstate.Services.RealEstateComAu
{
    public class ReaXmlTransmorgrifier : ITransmorgrifier
    {
        private static readonly IList<string> ValidRootNodes = new List<string>
        {
            "propertyList", "residential", "rental", "rural", "land"
        };

        private static readonly string[] ValidElementNodes =
        {
            "residential", "rental", "land", "rural"
        };

        private CultureInfo _defaultCultureInfo;

        public ReaXmlTransmorgrifier()
        {
            AddressDelimeter = "/";
        }

        /// <summary>
        /// Parses and converts some given data into a listing instance.
        /// </summary>
        /// <param name="data">some data source, like Xml data or json data.</param>
        /// <param name="existingListing">An optional destination listing which will extract any data, into.</param>
        /// <param name="areBadCharactersRemoved">Help clean up the data.</param>
        /// <returns>List of listings, unhandled data and/or errors.</returns>
        /// <remarks>Why does <code>isClearAllIsModified</code> default to <code>false</code>? Because when you generally load some data into a new listing instance, you want to see which properties </remarks>
        public ParsedResult Parse(string data,
            Listing existingListing = null,
            bool areBadCharactersRemoved = false)
        {
            Guard.AgainstNullOrWhiteSpace(data);

            var cleanedResult = CleanUpReaXmlData(data, areBadCharactersRemoved);
            if (cleanedResult.Item2.Any())
            {
                // We have an error, so we need to quit now.
                return new ParsedResult
                {
                    Errors = cleanedResult.Item2
                };
            }

            // Okay - so we have some valid (and possibly cleaned?) XML data.
            // Now split it up into the known listing types.
            SplitElementResult elements;

            try
            {
                elements = SplitReaXmlIntoElements(cleanedResult.Item1);
            }
            catch (Exception exception)
            {
                var error = new ParsedError(exception.Message,
                    "Failed to parse the provided xml data because it contains some invalid data. Pro Tip: This is usually because a character is not encoded. Like an ampersand.");
                return new ParsedResult(error);
            }

            // Do I have _anything_ ?
            if (elements == null)
            {
                // Nope - nothing to convert but also no errors :(
                return null;
            }

            // Parsed results...
            var successfullyParsedListings = new ConcurrentBag<ListingResult>();
            var invalidData = new ConcurrentBag<ParsedError>();

            if (elements.KnownElements.Any())
            {
                // Are we trying to copy REA Xml data into an existing listing?
                if (existingListing != null)
                {
                    // I can only copy over XML data from _one_ element into another listing.
                    // If I have multiple elements then I need to fail, now-and-early.
                    // Otherwise, which element do I use, to copy into?
                    if (elements.KnownElements.Count != 1)
                    {
                        // Yep - we're trying to parse a single REA Xml element into an existing listing
                        // but we have more than one REA Xml element! Oh noes!
                        return new ParsedResult(
                            new ParsedError(
                                $"You can only parse a single REA Xml element when you wish to update existing Listing instance. Currently, the REA Xml data contains {elements.KnownElements.Count} element(s). How to fix: please only provide one REA Xml to parse when updating an existing Listing instance.",
                                null));
                    }

                    // 1x REA Xml element.
                    // 1x existing listing.
                    // => Parse the REA Xml element into this existing listing.
                    ParseReaXmlElement(successfullyParsedListings,
                        invalidData,
                        elements.KnownElements.First(),
                        existingListing);
                }
                else
                {
                    // We don't have an existing listing BUT we have some element(s)...
                    // 1+ REA Xml elements.
                    // -no- existing listing.
                    // => Parse mutliple elements into newly created listing instances.
                    ParseReaXmlElements(successfullyParsedListings,
                        invalidData,
                        elements.KnownElements);
                }
            }

            // Ok. so we're finally finished parsing the xml.
            // What do we have?
            // We have either:
            //     - Possibly some listings.
            //     - Possibly some errors during _individual_ REA Xml segments.
            return new ParsedResult
            {
                Listings = successfullyParsedListings.ToList(),
                UnhandledData = elements.UnknownElements.Select(x => x.ToString()).ToList(),
                Errors = invalidData.ToList()
            };
        }

        private static Tuple<string, List<ParsedError>> CleanUpReaXmlData(string data,
            bool areBadCharactersRemoved)
        {
            Guard.AgainstNullOrWhiteSpace(data);

            var errors = new List<ParsedError>();

            // Remove any BOM if one exists.
            // REF: http://stackoverflow.com/questions/5098757/how-to-tell-asciiencoding-class-not-to-decode-the-byte-order-mark
            data = RemoveByteOrderMark(data);

            // Any validation errors in the xml?
            var validationErrorMessage = ValidateXmlString(data);
            if (!string.IsNullOrWhiteSpace(validationErrorMessage))
            {
                // Yep - we do have some validation errors.
                // But, can we try again and remove any bad characters?
                if (!areBadCharactersRemoved)
                {
                    // Nope - oh well. We're being strict so lets just quit now with an error.
                    errors.Add(new ParsedError(validationErrorMessage, "The entire data source."));
                    //return new Tuple<string, List<ParsedError>>(null, errors);
                }
                else
                {
                    // Some bad data occurs but we are allowed to try and clean any bad data out :)
                    data = RemoveInvalidXmlChars(data);
                }
            }

            // We only return the nice data when we don't have any errors.
            return new Tuple<string, List<ParsedError>>(errors.Any()
                ? null
                : data,
                errors);
        }

        private void ParseReaXmlElement(ConcurrentBag<ListingResult> successfullyParsedListings,
            ConcurrentBag<ParsedError> invalidData,
            XElement element,
            Listing existingListing = null)
        {
            Guard.AgainstNull(successfullyParsedListings);
            Guard.AgainstNull(invalidData);
            Guard.AgainstNull(element);

            try
            {
                var listing = ParseReaXml(element,
                    existingListing,
                    DefaultCultureInfo,
                    AddressDelimeter);
                successfullyParsedListings.Add(new ListingResult
                {
                    Listing = listing,
                    SourceData = element.ToString()
                });
            }
            catch (Exception exception)
            {
                invalidData.Add(new ParsedError(exception.Message, element.ToString()));
            }
        }

        private void ParseReaXmlElements(ConcurrentBag<ListingResult> successfullyParsedListings,
            ConcurrentBag<ParsedError> invalidData,
            IList<XElement> elements)
        {
            Guard.AgainstNull(successfullyParsedListings);
            Guard.AgainstNull(invalidData);
            Guard.AgainstNull(elements);

            Parallel.ForEach(elements, element =>
            {
                ParseReaXmlElement(successfullyParsedListings,
                    invalidData,
                    element);
            });
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
            Guard.AgainstNullOrWhiteSpace(text);

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
            Guard.AgainstNullOrWhiteSpace(text);

            var validXmlChars = text.Where(XmlConvert.IsXmlChar).ToArray();
            return new string(validXmlChars);
        }

        private static void EnsureXmlHasRootNode(ref XDocument document)
        {
            Guard.AgainstNull(document);

            var rootNode = document.Root?.Name.LocalName;

            if (string.IsNullOrWhiteSpace(rootNode) ||
                !ValidRootNodes.Contains(document.Root.Name.LocalName))
            {
                var errorMessage =
                    $"Unable to parse the xml data provided. Currently, only a <propertyList/> or listing segments <residential/> / <rental/> / <land/> / <rural/>. Root node found: '{(document.Root == null ? "-no root node" : document.Root.Name.LocalName)}'.";
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
            Guard.AgainstNullOrWhiteSpace(xml);

            // If there are bad elements in the XML, then this throw an exception.
            // Eg. & (ampersands) in video links that are not properly encoded, etc.
            var document = XDocument.Parse(xml);

            // Prepare the xml data we're given.
            EnsureXmlHasRootNode(ref document);

            // Split the elements into ones we know and car parse, or unknowns/unhandled elements.
            var knownElements = document?.Root?.Elements()
                .Where(x => ValidElementNodes
                    .Any(node => string.Compare(node, x.Name.LocalName, true, CultureInfo.InvariantCulture) == 0))
                .ToList();

            var unknownElements = document?.Root?.Elements()
                .Where(x => ValidElementNodes
                    .All(node => string.Compare(node, x.Name.LocalName, true, CultureInfo.InvariantCulture) != 0))
                .ToList();

            return knownElements == null &&
                unknownElements == null
                ? null
                : new SplitElementResult
                {
                    KnownElements = knownElements,
                    UnknownElements = unknownElements 
                };
        }

        private static Listing ParseReaXml(XElement document,
            Listing existingListing, 
            CultureInfo cultureInfo,
            string addressDelimeter)
        {
            Guard.AgainstNull(document);

            // Determine the category, so we know why type of listing we need to create.
            var categoryType = document.Name.ToCategoryType();

            // We can only handle a subset of all the category types.
            var listing = existingListing ?? CreateListing(categoryType);
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

        private static DateTime ToDateTime(string reaDateTime, string elementName = null)
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

            // FFS REA!!!! URGH!!!!!!!! :( 
            // Stick to fricking ISO8061 with yyyy-MM-ddTHH:mm:ss 
            // ONE FORMAT TO RULE THEM ALL.
            // (not that hard, peeps).
            */
            var formats = new[]
            {
                "yyyy-MM-dd",
                "yyyy-MM-dd-HH:mm",
                "yyyy-MM-ddTHH:mm",
                "yyyy-MM-dd-HH:mm:",
                "yyyy-MM-dd-HH:mm:ss",
                "yyyy-MM-ddTHH:mm:ss",
                "yyyy-MM-ddTH:mm:ss", // 2016-05-21T9:33:49 (Notice the single 'hour' that is not 24 hour format?)
                "yyyy-MM-dd-hh:mm:sstt", // 2015-12-15-01:18:52PM
                "yyyyMMdd-HHmmss",
                "yyyyMMDD-HHmmss",
                "yyyyMMddTHHmmss",
                "yyyyMMDDTHHmm",
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

            var errorMessage =
                AppendElementOrAttributeToErrorMessage(
                    $"Invalid date/time trying to be parsed. Attempted the value: '{reaDateTime}' but that format is invalid.",
                    elementName);
            throw new Exception(errorMessage);
        }

        #region Common listing methods

        private static void ExtractCommonData(Listing listing, 
            XElement document, 
            string addressDelimeter)
        {
            Guard.AgainstNull(listing);
            Guard.AgainstNull(document);

            listing.UpdatedOn = ToDateTime(document.AttributeValue("modTime"), "<root modTime>");

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

            document.ValueOrDefaultIfExists(title => listing.Title = title, "headline");
            document.ValueOrDefaultIfExists(description => listing.Description = description, "description");

            ExtractAddress(document, listing, addressDelimeter);
            ExtractAgents(document, listing);
            ExtractInspectionTimes(document, listing);
            ExtractImages(document, listing);
            ExtractFloorPlans(document, listing);
            ExtractVideos(document, listing);
            ExtractFeatures(document, listing);
            ExtractLandDetails(document, listing);
            ExtractExternalLinks(document, listing);
        }

        // NOTE: if an Address element exist, then it is assumed
        //       that _all_ the required info will be there, not just 
        //       _some_ info.
        //       As such, this means I'm going to just hard-set the 
        //       element values, not set-if-element-exist.
        //       WHY? Some of our properties require a mix of multi-REA-elements.
        //       eg. Lot / Sub numbers...
        private static void ExtractAddress(XElement document, 
            Listing listing,
            string addressDelimeter)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            var addressElement = document.Element("address");
            if (addressElement == null)
            {
                return;
            }

            // Have we created an address before?
            if (listing.Address == null)
            {
                listing.Address = new Address();
            }

            // Land and CommericalLand should only provide lot numbers. 
            var lotNumber = addressElement.ValueOrDefault("lotNumber");
            var subNumber = addressElement.ValueOrDefault("subNumber");
            var streetNumber = addressElement.ValueOrDefault("streetNumber");

            // LOGIC:
            // So, we're trying to create a streetnumber value that contains the REA values
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
                    : $"LOT {lotNumber}";

            // Sub number and Street number logic: A sub number can exist -before- the street number.
            // A street number might NOT exist, so a sub number is all by itself.
            // When we want to show a sub number, we probably want to show it, like this:
            //    'subNumber`delimeter`streetNumber`
            //   eg. 12a/432
            // But .. sometimes, the sub number -already- contains a delimeter! so then we want this:
            //   eg. 45f/231 15
            // So we don't put a delimeter in there, but a space. Urgh! confusing, so sowwy.

            var subNumberLotNumber = $"{subNumber} {lotNumberResult}".Trim();

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

            listing.Address.StreetNumber = $"{subNumberLotNumber}{delimeter}{streetNumber}".Trim();

            listing.Address.Street = addressElement.ValueOrDefault("street");
            listing.Address.Suburb = addressElement.ValueOrDefault("suburb");
            listing.Address.State = addressElement.ValueOrDefault("state");

            // REA Xml Rule: Country is ommited == default to Australia.
            // Reference: http://reaxml.realestate.com.au/docs/reaxml1-xml-format.html#country
            var country = addressElement.ValueOrDefault("country");
            listing.Address.CountryIsoCode = !string.IsNullOrEmpty(country)
                ? ParseCountryToIsoCode(country)
                : "AU";

            listing.Address.Postcode = addressElement.ValueOrDefault("postcode");

            var isStreetDisplayedText = addressElement.AttributeValueOrDefault("display");
            listing.Address.IsStreetDisplayed = string.IsNullOrWhiteSpace(isStreetDisplayedText) ||
                                        addressElement.AttributeBoolValueOrDefault("display");


            // Technically, the <municipality/> element is not a child of the <address/> element.
            // But I feel that it's sensible to still parse for it, in here.
            document.ValueOrDefaultIfExists(municipality => listing.Address.Municipality = municipality, "municipality");

            // Finally - Lat/Longs. These are -not- part of the REA XML standard.
            // ~BUT~ some multi-loaders are sticking this data into some xml!
            ExtractLatitudeLongitudes(document, listing.Address);
        }

        private static void ExtractLatitudeLongitudes(XContainer document, Address address)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(address);

            var latitudeElement = document.Descendants("Latitude").FirstOrDefault() ??
                                  document.Descendants("latitude").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(latitudeElement?.Value))
            {
                address.Latitude = latitudeElement.DecimalValueOrDefault();
            }

            var longitudeElement = document.Descendants("Longitude").FirstOrDefault() ??
                                   document.Descendants("longitude").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(longitudeElement?.Value))
            {
                address.Longitude = longitudeElement.DecimalValueOrDefault();
            }
        }

        // NOTE: This is a extracting a collection. As such, we hard-copy the resultant collection
        //       over to the listing. No guessing or matching.
        private static void ExtractAgents(XContainer document, Listing listing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            var agentElements = document.Elements("listingAgent").ToArray();
            if (!agentElements.Any())
            {
                return;
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
                int order;
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

                agent.Communications = communications;

                // Don't add this agent, if the name already exists in the list.
                if (!agents.Any(x => x.Name.Equals(agent.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    // This agent doesn't exists - so we're good to add them!
                    agents.Add(agent);
                }
            }

            if (agents.Any())
            {
                var order = 0;

                // Order the results :)
                listing.Agents = agents
                    .Select(x => new ListingAgent
                    {
                        Name = x.Name,
                        Order = ++order,
                        Communications = x.Communications
                    })
                    .OrderBy(x => x.Order).ToList();
            }
        }

        // NOTE: If this element exist, then we hard-copy everything.
        //       We don't guess which features are now provided.
        private static void ExtractFeatures(XContainer document, Listing listing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            var featuresElement = document.Element("features");
            if (featuresElement == null)
            {
                // No features element, so just move along...
                return;
            }

            // Have we made any features yet?
            var tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // NOTE: Bedrooms can be a number -or- the value 'STUDIO'.
            //       YES - where a number is the logical value, they can now have a string. :cry:
            //       So, if the value is a string, like STUDIO (or anything else), then the
            //       value will be returned as ZERO.
            //       If it's a STUDIO, we'll add that to the feature's tag hash set.
            var bedroomsValue = featuresElement.ValueOrDefault("bedrooms");
            byte bedrooms = 0;
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
                Tags = tags
            };

            listing.Features = finalFeatures;
        }

        private static void ExtractFeatureWithTextValues(XElement document, 
            string elementName, 
            string[] validValues,
            ISet<string> tags,
            string delimeter = "-")
        {
            Guard.AgainstNull(document);
            Guard.AgainstNullOrWhiteSpace(elementName);
            Guard.AgainstNull(validValues);
            Guard.AgainstNull(tags);

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
            Guard.AgainstNull(document);
            Guard.AgainstNull(tags);

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

        // NOTE: This is a extracting a collection. As such, we hard-copy the resultant collection
        //       over to the listing. No guessing or matching.
        private static void ExtractInspectionTimes(XContainer document, Listing listing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            var inspectionTimes = document.Element("inspectionTimes");
            if (inspectionTimes == null)
            {
                return;
            }

            var inspectionElements = inspectionTimes.Elements("inspection").ToList();
            if (!inspectionElements.Any())
            {
                return;
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

                DateTime.TryParse($"{data[0]} {data[1]}", out inspectionStartsOn);
                DateTime.TryParse($"{data[0]} {data[3]}", out inspectionEndsOn);

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

            listing.Inspections = inspections.OrderBy(x => x.OpensOn).ToList();
        }

        // NOTE: This is a extracting a collection. As such, we hard-copy the resultant collection
        //       over to the listing. No guessing or matching.
        private static void ExtractImages(XContainer document, Listing listing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            // Images can go in either <images> or <objects>. Only idiots are putting them in <objects>
            // Generally, <objects> is reservered for floorplans. :/
            var imageElement = document.Element("images") ?? document.Element("objects");
            if (imageElement == null)
            {
                return;
            }

            var imagesElements = imageElement.Elements("img").ToArray();
            if (!imagesElements.Any())
            {
                return;
            }

            listing.Images = ParseMediaXmlDataToMedia(imagesElements, ConvertImageOrderToNumber, "img")
                .ToList();
        }

        // NOTE: This is a extracting a collection. As such, we hard-copy the resultant collection
        //       over to the listing. No guessing or matching.
        private static void ExtractFloorPlans(XContainer document, Listing listing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            // NOTE: The idea is that <images> will contain images while <objects> will only be for floorplans.
            //       Yes, some listings put their images in <objects>, but this is handled elsewhere.
            var objectElement = document.Element("objects");
            if (objectElement == null)
            {
                return;
            }

            var floorPlanElements = objectElement.Elements("floorplan").ToArray();
            if (!floorPlanElements.Any())
            {
                return;
            }

            listing.FloorPlans = ParseMediaXmlDataToMedia(floorPlanElements, Convert.ToInt32, "floorplan")
                .ToList();
        }

        private static IEnumerable<Media> ParseMediaXmlDataToMedia(IEnumerable<XElement> mediaElements, 
            Func<string, int> orderConverstionFunction,
            string elementName = null)
        {
            // Note 1: Image 'urls' can either be via a Uri (yay!) or
            //         a file name because the xml was provided in a zip file with
            //         the images (booooo! hiss!!!)
            // Note 2: Not all image's might have a last mod time.
            return (from x in mediaElements
                let url = x.AttributeValueOrDefault("url")
                let file = x.AttributeValueOrDefault("file")
                let order = x.AttributeValueOrDefault("id")
                let createdOn = x.AttributeValueOrDefault("modTime")
                where (!string.IsNullOrWhiteSpace(url) ||
                       !string.IsNullOrWhiteSpace(file)) &&
                      !string.IsNullOrWhiteSpace(order)
                select new Media
                {
                    CreatedOn = string.IsNullOrWhiteSpace(createdOn)
                        ? (DateTime?) null
                        : ToDateTime(createdOn, $"<{elementName} modTime='..'/>"),
                    Url = string.IsNullOrWhiteSpace(url)
                        ? string.IsNullOrWhiteSpace(file)
                            ? null
                            : file
                        : url,
                    Order = orderConverstionFunction(order)
                }).ToList();
        }

        // NOTE: This is a extracting a collection. As such, we hard-copy the resultant collection
        //       over to the listing. No guessing or matching.
        private static void ExtractVideos(XElement document, Listing listing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            // What we need to do, if the element is found.
            var action = new Action<string>(videoUrl =>
            {
                var media = new List<Media>();
                if (!string.IsNullOrWhiteSpace(videoUrl))
                {
                    media.Add(new Media
                    {
                        CreatedOn = null, // We don't have this data in the REA Xml file.
                        Order = 1,
                        Url = videoUrl
                    });
                }

                listing.Videos = media;
            });

            document.ValueOrDefaultIfExists(action, "videoLink", "href");
        }

        private static void ExtractResidentialAndRentalPropertyType(XElement document, IPropertyType listing)
        {
            Guard.AgainstNull(listing);

            document.ValueOrDefaultIfExists(
                propertyType => listing.PropertyType = PropertyTypeHelpers.ToPropertyType(propertyType),
                "category",
                "name");
        }

        private static void ExtractSalePricing(XElement document,
            ISalePricing listing,
            CultureInfo cultureInfo)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            if (document.Element("price") == null &&
                document.Element("priceView") == null &&
                document.Element("soldDetails") == null)
            {
                // No pricing elements - move along!
                return;
            }

            // Do we have an existing sale pricing?
            if (listing.Pricing == null)
            {
                listing.Pricing = new SalePricing();
            }

            ExtractSalePrice(document, listing.Pricing, cultureInfo);

            ExtractSoldDetails(document, listing.Pricing);
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
            Guard.AgainstNull(document);
            Guard.AgainstNull(salePricing);

            document.MoneyValueOrDefaultIfExists(salePrice => salePricing.SalePrice = salePrice,
                cultureInfo,
                "price");

            // ### MASSIVE NOTICE ###
            // This is where shit gets real. :/
            // <Price> and <PriceView> are optional. :/
            // If we have a <PriceView> .. we need to do all our normal hardcore crazy tests and shit.
            // Otherwise, we need to just work off the <Price> .. _if_ we have one.

            var priceViewElement = document.Element("priceView");
            var priceElement = document.Element("price");
            if (priceViewElement != null)
            {
                // We have a priceView element.
                // We also assume that if we have a priceView element, we have a price element.
                CalculateSalePriceWhenPriceViewElementExists(document, salePricing);
            }
            else if (priceElement != null)
            {
                // We don't have a priceView element, but we do have a price element.. so we can calc the
                // display text, still.
                CalculateSalePriceWhenPriceElementExists(document, salePricing);
            }

            document.ValueOrDefaultIfExists(isUnderOffer =>
            {
                salePricing.IsUnderOffer = !string.IsNullOrWhiteSpace(isUnderOffer) &&
                                           isUnderOffer.ParseOneYesZeroNoToBool();
            },
                "underOffer",
                "value");
        }

        private static void CalculateSalePriceWhenPriceViewElementExists(XElement document, SalePricing salePricing)
        {
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

        private static void CalculateSalePriceWhenPriceElementExists(XElement document, SalePricing salePricing)
        {
            var displayAttributeValue = document.ValueOrDefault("price", "display");
            var isDisplay = string.IsNullOrWhiteSpace(displayAttributeValue) ||
                            displayAttributeValue.ParseOneYesZeroNoToBool();

            // NOTE 1: If Display="no" then we do not display anything for the price, regardless
            //       of any other data provided. Otherwise, make a decision.
            // NOTE 2: If -NO- saleprice is provided (eg. this is _very_ common when we get
            //         an SOLD or LEASED, etc) then we should leave the sale price text alone.
            //         So only do the sale-price-text checks if we have a value set AND
            //         it's ok to display a value.
            // NOTE 3: display='no' means NO price is displayed, even if there's a priceText.
            salePricing.SalePriceText = isDisplay 
                ? salePricing.SalePrice.ToString("C0")
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
        private static void ExtractSoldDetails(XContainer document, SalePricing salePricing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(salePricing);

            var soldDetails = document.Element("soldDetails");
            if (soldDetails == null)
            {
                // No element so move along...
                return;
            }

            // Yay! we have some sold details...

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

        // Eg xml: <price display="yes">580000</price>
        private static void ExtractSoldPrice(XElement document, SalePricing salePricing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(salePricing);

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
            Guard.AgainstNull(document);

            // SoldOn could be date or soldData. Thanks REA for such a great schema.
            var soldOnText = document.ValueOrDefault();
            if (!string.IsNullOrWhiteSpace(soldOnText))
            {
                salePricing.SoldOn = ToDateTime(soldOnText, "<soldDetails><date/><soldDetails>");
            }
        }

        private static void ExtractAuction(XElement document, IAuctionOn listing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            var action = new Action<string>(auction =>
            {
                // NOTE: The REA documentation is vague as to the 100% specification on this.
                // So i'm going to assume the following (in order)
                // 1. <auction>date-time-in-here</auction>
                // 2. <auction date="date-time-in-here"></auction>
                // ** YET ANOTHER FRICKING EXAMPLE OF WHY THIS SCHEMA AND XML ARE F'ING CRAP **
                if (string.IsNullOrWhiteSpace(auction))
                {
                    auction = document.ValueOrDefault("auction", "date");
                }

                listing.AuctionOn = !string.IsNullOrWhiteSpace(auction)
                    ? (DateTime?) ToDateTime(auction, "<auction/> or <auction date=''/>")
                    : null;
            });

            document.ValueOrDefaultIfExists(action, "auction");
        }

        private static void ExtractBuildingDetails(XContainer document, IBuildingDetails listing)
        {
            Guard.AgainstNull(document);

            var buildingDetailsElement = document.Element("buildingDetails");
            if (buildingDetailsElement == null)
            {
                return;
            }

            var energyRating = buildingDetailsElement.DecimalValueOrDefault("energyRating");

            listing.BuildingDetails = new BuildingDetails
            {
                Area = buildingDetailsElement.UnitOfMeasureOrDefault("area", "unit"),
                EnergyRating = energyRating <= 0
                    ? (decimal?) null
                    : energyRating,
            };
        }

        private static void ExtractLandDetails(XContainer document, Listing listing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            var landDetailsElement = document.Element("landDetails");
            if (landDetailsElement == null)
            {
                // Element not found - move along.
                return;
            }

            var details = new LandDetails
            {
                Area = landDetailsElement.UnitOfMeasureOrDefault("area", "unit"),
                Frontage = landDetailsElement.UnitOfMeasureOrDefault("frontage", "unit"),
                CrossOver = landDetailsElement.ValueOrDefault("crossOver", "value"),
                Depths = new List<Depth>()
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

                        details.Depths.Add(depth);
                    }
                }
            }

            listing.LandDetails = details;
        }

        private static void ExtractExternalLinks(XContainer document, Listing listing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            var elements = document.Elements("externalLink").ToArray();
            if (!elements.Any())
            {
                // Nothing to see here.
                return;
            }

            listing.Links = (from e in elements
                let externalLink = e.AttributeValueOrDefault("href")
                where !string.IsNullOrWhiteSpace(externalLink)
                select Uri.UnescapeDataString(externalLink.Trim()))
                .ToList();
        }

        ///// <summary>
        ///// REA Specific DateTime parsing.
        ///// </summary>
        //private static DateTime ParseReaDateTime(string someDateTime)
        //{
        //    Guard.AgainstNullOrWhiteSpace(someDateTime);

        //    // FFS REA!!!! URGH!!!!!!!! :( 
        //    // Stick to fricking ISO8061 with yyyy-MM-ddTHH:mm:ss 
        //    // ONE FORMAT TO RULE THEM ALL.
        //    // (not that hard, peeps).
        //    var reaDateTimeFormats = new[]
        //    {
        //        "yyyy-MM-dd",
        //        "yyyy-MM-dd-HH:mm",
        //        "yyyy-MM-dd-HH:mm:ss",
        //        "yyyy-MM-dd-hh:mm:sstt", // 2015-12-15-01:18:52PM
        //        "yyyy-MM-ddTHH:mm",
        //        "yyyy-MM-ddTHH:mm:ss",
        //        "yyyyMMdd",
        //        "yyyyMMdd-HHmm",
        //        "yyyyMMDD-HHmmss",
        //        "yyyyMMDDTHHmm",
        //        "yyyyMMddTHHmmss"
        //    };

        //    DateTime resultDateTime;

        //    if (!DateTime.TryParse(someDateTime, out resultDateTime))
        //    {
        //        DateTime.TryParseExact(someDateTime.Trim(), reaDateTimeFormats, CultureInfo.InvariantCulture,
        //            DateTimeStyles.None, out resultDateTime);
        //    }

        //    return resultDateTime;
        //}

        //private static DateTime? ParseReaDateTimeNullable(string someDateTime)
        //{
        //    Guard.AgainstNullOrWhiteSpace(someDateTime);

        //    var result = ParseReaDateTime(someDateTime);
        //    return result == DateTime.MinValue
        //        ? (DateTime?)null
        //        : result;
        //}

        private static string ParseCountryToIsoCode(string country)
        {
            Guard.AgainstNullOrWhiteSpace(country);

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

        private static string AppendElementOrAttributeToErrorMessage(string errorMessage,
            string elementOrAttributeName = null)
        {
            if (!string.IsNullOrWhiteSpace(elementOrAttributeName))
            {
                errorMessage += $" Element/Attribute: {elementOrAttributeName}";
            }

            return errorMessage;
        }

        #endregion

        #region Residential Listing methods

        private static void ExtractResidentialData(ResidentialListing residentialListing,
            XElement document, 
            CultureInfo cultureInfo)
        {
            Guard.AgainstNull(residentialListing);
            Guard.AgainstNull(document);

            // Create interfaces for PropertyType, SalePrice, etc.
            // Update the Res and Rental.
            // replace action iwth IPropertyType, ISalePricing, etc.

            ExtractResidentialAndRentalPropertyType(document, residentialListing);
            ExtractSalePricing(document, residentialListing, cultureInfo);
            ExtractAuction(document, residentialListing);
            ExtractBuildingDetails(document, residentialListing);
            document.ValueOrDefaultIfExists(councilRates => residentialListing.CouncilRates  = councilRates, "councilRates");
            ExtractHomeAndLandPackage(document, residentialListing);
            ExtractResidentialNewConstruction(document, residentialListing);
        }

        private static void ExtractHomeAndLandPackage(XContainer document, Listing residentialListing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(residentialListing);

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

                residentialListing.Features.Tags.Add("houseAndLandPackage");
            }
        }

        private static void ExtractResidentialNewConstruction(XElement document, ResidentialListing listing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            if (!document.BoolValueOrDefault("newConstruction"))
            {
                return;
            }

            if (listing.Features == null)
            {
                listing.Features= new Features();
            }

            listing.Features.Tags.Add("isANewConstruction");
        }

        #endregion

        #region Rental Listing Methods

        private static void ExtractRentalData(RentalListing rentalListing, XElement document, CultureInfo cultureInfo)
        {
            Guard.AgainstNull(rentalListing);
            Guard.AgainstNull(document);

            var dateAvailble = document.ValueOrDefault("dateAvailable");
            if (!string.IsNullOrWhiteSpace(dateAvailble))
            {
                rentalListing.AvailableOn = ToDateTime(dateAvailble, "<rental><dateAvailable/><rental>");
            }

            ExtractResidentialAndRentalPropertyType(document, rentalListing);
            rentalListing.Pricing = ExtractRentalPricing(document, cultureInfo);
            ExtractFeatures(document, rentalListing);
            ExtractBuildingDetails(document, rentalListing);
            ExtractRentalNewConstruction(document, rentalListing);
        }

        // REF: http://reaxml.realestate.com.au/docs/reaxml1-xml-format.html#rent
        private static RentalPricing ExtractRentalPricing(XElement xElement, CultureInfo cultureInfo)
        {
            Guard.AgainstNull(xElement);


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

        private static void ExtractRentalNewConstruction(XElement document, Listing listing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            if (!document.BoolValueOrDefault("newConstruction"))
            {
                return;
            }

            if (listing.Features == null)
            {
                listing.Features = new Features();
            }

            listing.Features.Tags.Add("isANewConstruction");
        }

        #endregion

        #region Land Listing Methods

        private static void ExtractLandData(LandListing landListing, XElement document, CultureInfo cultureInfo)
        {
            Guard.AgainstNull(landListing);
            Guard.AgainstNull(document);

            landListing.CategoryType = ExtractLandCategoryType(document);
            ExtractSalePricing(document, landListing, cultureInfo);
            ExtractAuction(document, landListing);
            landListing.Estate = ExtractLandEstate(document);
            landListing.CouncilRates = document.ValueOrDefault("councilRates");
        }

        private static Core.Land.CategoryType ExtractLandCategoryType(XElement xElement)
        {
            var category = xElement.ValueOrDefault("landCategory", "name");
            return string.IsNullOrWhiteSpace(category)
                ? Core.Land.CategoryType.Unknown
                : CategoryTypeHelpers.ToCategoryType(category);
        }

        private static LandEstate ExtractLandEstate(XElement xElement)
        {
            Guard.AgainstNull(xElement);

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
            Guard.AgainstNull(document);

            ruralListing.CategoryType = ExtractRuralCategoryType(document);
            ExtractSalePricing(document, ruralListing, cultureInfo);
            ExtractAuction(document, ruralListing);
            ruralListing.RuralFeatures = ExtractRuralFeatures(document);
            ruralListing.CouncilRates = ExtractRuralCouncilRates(document);
            ExtractBuildingDetails(document, ruralListing);
            ExtractRuralNewConstruction(document, ruralListing);
        }

        private static Core.Rural.CategoryType ExtractRuralCategoryType(XContainer document)
        {
            Guard.AgainstNull(document);

            var categoryElement = document.Element("ruralCategory");
            if (categoryElement == null)
            {
                return Core.Rural.CategoryType.Unknown;
            }

            var categoryValue = categoryElement.AttributeValueOrDefault("name");
            return string.IsNullOrWhiteSpace(categoryValue)
                ? Core.Rural.CategoryType.Unknown
                : Core.Rural.CategoryTypeHelpers.ToCategoryType(categoryValue);
        }

        private static RuralFeatures ExtractRuralFeatures(XContainer document)
        {
            Guard.AgainstNull(document);
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

        private static string ExtractRuralCouncilRates(XContainer document)
        {
            Guard.AgainstNull(document);
            var ruralFeaturesElement = document.Element("ruralFeatures");
            return ruralFeaturesElement?.ValueOrDefault("councilRates");
        }

        private static void ExtractRuralNewConstruction(XElement document, Listing listing)
        {
            Guard.AgainstNull(document);
            Guard.AgainstNull(listing);

            if (!document.BoolValueOrDefault("newConstruction"))
            {
                return;
            }

            if (listing.Features == null)
            {
                listing.Features = new Features();
            }

            listing.Features.Tags.Add("isANewConstruction");
        }

        #endregion

        private class SplitElementResult
        {
            public IList<XElement> KnownElements { get; set; }
            public IList<XElement> UnknownElements { get; set; }
        }
    }
}