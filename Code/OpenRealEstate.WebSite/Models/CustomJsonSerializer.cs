using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenRealEstate.WebSite.Models
{
    public class CustomJsonSerializer : JsonSerializer
    {
        public CustomJsonSerializer()
        {
            Converters.Add(new StringEnumConverter());
        }
    }
}