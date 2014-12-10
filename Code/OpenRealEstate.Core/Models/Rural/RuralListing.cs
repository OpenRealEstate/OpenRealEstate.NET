using System;

namespace OpenRealEstate.Core.Models.Rural
{
    public class RuralListing : Listing
    {
        public CategoryType CategoryType { get; set; }
        public SalePricing Pricing { get; set; }
        public DateTime? AuctionOn { get; set; }
        public RuralFeatures RuralFeatures { get; set; }
        public string CouncilRates { get; set; }
        public BuildingDetails BuildingDetails { get; set; }

        public override string ToString()
        {
            return string.Format("Rural >> {0}", base.ToString());
        }
    }
}