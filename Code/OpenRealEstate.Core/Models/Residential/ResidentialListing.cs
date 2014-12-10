using System;

namespace OpenRealEstate.Core.Models.Residential
{
    public class ResidentialListing : Listing
    {
        public PropertyType PropertyType { get; set; }
        public SalePricing Pricing { get; set; }
        public DateTime? AuctionOn { get; set; }
        public string CouncilRates { get; set; }

        public override string ToString()
        {
            return string.Format("Residential >> {0}", base.ToString());
        }
    }
}