using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Services.Json
{
    public class JsonTransmorgrifier : ITransmorgrifier
    {
        private static JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings
        {
            Converters = new JsonConverter[]
            {
                new ListingConverter()
            },
            ContractResolver = new ListingContractResolver()
        };

        public ConvertToResult ConvertTo(string data,
            bool areBadCharactersRemoved = false,
            bool isClearAllIsModified = false)
        {
            Guard.AgainstNullOrWhiteSpace(data);

            var result = new ConvertToResult();

            JToken token;

            try
            {
                token = JToken.Parse(data);
            }
            catch (Exception exception)
            {
                result.Errors = new[] {new ParsedError(exception.Message, data)};
                return result;
            }

            if (token is JArray)
            {
                foreach (var item in token.Children())
                {
                    var convertToResult = ParseObject(item.ToString());
                    MergeConvertToResults(convertToResult, result);
                }
            }
            else
            {
                var convertToResult = ParseObject(data);
                MergeConvertToResults(convertToResult, result);
            }

            return result;
        }

        private static ConvertToResult ParseObject(string json)
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

            return new ConvertToResult
            {
                Listings = listing == null
                    ? null
                    : new List<ListingResult> {new ListingResult {Listing = listing, SourceData = json}},
                Errors = error == null
                    ? null
                    : new List<ParsedError> {new ParsedError(error.Message, json)}
            };
        }

        private static void MergeConvertToResults(ConvertToResult source, ConvertToResult destination)
        {
            source.ShouldNotBeNull();
            destination.ShouldNotBeNull();

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