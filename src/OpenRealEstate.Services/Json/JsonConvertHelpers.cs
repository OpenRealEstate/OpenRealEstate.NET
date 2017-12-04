using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenRealEstate.Core;

namespace OpenRealEstate.Services.Json
{
    public static class JsonConvertHelpers
    {
        private static JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings
        {
            Converters = new JsonConverter[]
            {
                new StringEnumConverter()
            },
            Formatting = Formatting.Indented
        };

        public static string SerializeObject(Listing listing)
        {
            return JsonConvert.SerializeObject(listing, JsonSerializerSettings);
        }

        public static string SerializeObject(IEnumerable<Listing> listings)
        {
            return JsonConvert.SerializeObject(listings, JsonSerializerSettings);
        }
    }
}