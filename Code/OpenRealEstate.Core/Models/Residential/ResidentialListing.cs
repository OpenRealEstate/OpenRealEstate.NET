using System;

namespace OpenRealEstate.Core.Models.Residential
{
    public class ResidentialListing : Listing
    {
        public PropertyType PropertyType { get; set; }
        public SalePricing Pricing { get; set; }
        public DateTime? AuctionOn { get; set; }
    }
}