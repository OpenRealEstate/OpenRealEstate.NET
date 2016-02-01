using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenRealEstate.Core.Models;
using Shouldly;

namespace OpenRealEstate.Services.Json
{
    public class JsonTransmorgrifier : ITransmorgrifier
    {
        public ConvertToResult ConvertTo(string data,
            bool areBadCharactersRemoved = false,
            bool isClearAllIsModified = false)
        {
            data.ShouldNotBeNullOrEmpty();

            Exception error = null;
            Listing listing = null;

            try
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    Converters = new JsonConverter[] { new ListingConverter() },
                    ContractResolver = new ListingContractResolver()
                };

                listing = JsonConvert.DeserializeObject<Listing>(data, jsonSettings);
            }
            catch (Exception exception)
            {
                error = exception;
            }

            return new ConvertToResult
            {
                Listings = listing == null
                    ? null
                    : new List<ListingResult> {new ListingResult {Listing = listing, SourceData = data}},
                Errors = error == null
                    ? null
                    : new List<ParsedError> {new ParsedError(error.Message, data)}
            };
        }
    }
}