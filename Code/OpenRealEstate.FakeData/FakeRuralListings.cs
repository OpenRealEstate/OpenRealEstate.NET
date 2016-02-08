using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRealEstate.Core.Models.Rural;
using OpenRealEstate.FakeData.Properties;
using OpenRealEstate.Services.Json;

namespace OpenRealEstate.FakeData
{
    public static class FakeRuralListings
    {
        public static IEnumerable<RuralListing> CreateFixedRuralListings()
        {
            var json = Encoding.UTF8.GetString(Resources.RuralListingsJson);

            var jsonTransmorgrifier = new JsonTransmorgrifier();
            var listings = jsonTransmorgrifier.ConvertTo(json);
            return listings.Listings.Select(x => x.Listing).Cast<RuralListing>();
        }
    }
}