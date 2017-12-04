using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenRealEstate.Core;

namespace OpenRealEstate.Services.Json
{
    public class JsonTransmorgrifier : ITransmorgrifier
    {
        private static JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings
        {
            Converters = new JsonConverter[]
            {
                new ListingConverter()
            }
        };

        /// <inheritdoc />
        public ParsedResult Parse(string data,
                                  Listing existingListing = null,
                                  bool areBadCharactersRemoved = false)
        {
            Guard.AgainstNullOrWhiteSpace(data);

            var result = new ParsedResult();

            JToken token;

            try
            {
                token = JToken.Parse(data);
            }
            catch (Exception exception)
            {
                result.Errors = new[]
                {
                    new ParsedError(exception.Message, data)
                };
                return result;
            }

            // Do we have a single listing or an array of listings?
            if (token is JArray)
            {
                // We have multiple listings...
                foreach (var item in token.Children())
                {
                    var parsedResult = ParseObject(item.ToString());
                    MergeParsedResults(parsedResult, result);
                }
            }
            else
            {
                // We have just a single listing ...
                var parsedResult = ParseObject(data);
                MergeParsedResults(parsedResult, result);
            }

            return result;
        }

        private static ParsedResult ParseObject(string json)
        {
            Listing listing = null;
            Exception error = null;
            try
            {
                listing = JsonConvert.DeserializeObject<Listing>(json, JsonSerializerSettings);
            }
            catch (Exception exception)
            {
                error = exception;
            }

            return new ParsedResult
            {
                Listings = listing == null
                               ? null
                               : new List<ListingResult>
                               {
                                   new ListingResult
                                   {
                                       Listing = listing,
                                       SourceData = json
                                   }
                               },
                Errors = error == null
                             ? null
                             : new List<ParsedError>
                             {
                                 new ParsedError(error.Message, json)
                             }
            };
        }

        private static void MergeParsedResults(ParsedResult source,
                                               ParsedResult destination)
        {
            if (source.Listings != null &&
                source.Listings.Any())
            {
                foreach (var listingResult in source.Listings)
                {
                    if (destination.Listings == null)
                    {
                        destination.Listings = new List<ListingResult>();
                    }

                    destination.Listings.Add(listingResult);
                }
            }

            if (source.Errors != null &&
                source.Errors.Any())
            {
                foreach (var parsedError in source.Errors)
                {
                    if (destination.Errors == null)
                    {
                        destination.Errors = new List<ParsedError>();
                    }

                    destination.Errors.Add(parsedError);
                }
            }
        }
    }
}