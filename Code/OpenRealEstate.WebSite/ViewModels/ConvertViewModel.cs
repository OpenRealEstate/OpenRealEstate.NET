using System.Collections.Generic;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.WebSite.ViewModels
{
    public class ConvertViewModel
    {
        public int ResidentialCount { get; set; }
        public int RentalCount { get; set; }
        public int RuralCount { get; set; }
        public int LandCount { get; set; }
        public IEnumerable<Listing> Listings { get; set; }
        public IDictionary<string, string> ValidationErrors { get; set; }
    }
}