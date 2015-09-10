using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Tests
{
    internal class ModifiedDataContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
            var x = props.Where(p => p.PropertyType != typeof (ModifiedData)).ToList();
            return x;
        }
    }
}