using System;

namespace OpenRealEstate.Core.Land
{
    public class LandListing : Listing, ISalePricing, IAuctionOn
    {
        public override string ListingType => "Land";

        public CategoryType CategoryType { get; set; }

        public LandEstate Estate { get; set; }

        public string CouncilRates { get; set; }

        public DateTime? AuctionOn { get; set; }

        public SalePricing Pricing { get; set; }

        public override string ToString()
        {
            return $"Land >> {base.ToString()}";
        }
    }
}