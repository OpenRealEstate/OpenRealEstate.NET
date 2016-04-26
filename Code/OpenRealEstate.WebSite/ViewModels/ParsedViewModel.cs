using System.Collections.Generic;

namespace OpenRealEstate.WebSite.ViewModels
{
    public class ParsedViewModel
    {
        public int ResidentialCount { get; set; }

        public int RentalCount { get; set; }

        public int RuralCount { get; set; }

        public int LandCount { get; set; }

        public string ListingsJson { get; set; }

        public IDictionary<string, string> ValidationErrors { get; set; }
    }
}