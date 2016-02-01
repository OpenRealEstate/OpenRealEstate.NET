using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Services.Json
{
    public class ListingContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type,
            MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            return properties.Where(p => p.PropertyType != typeof (ModifiedData)).ToArray();
        }
    }
}