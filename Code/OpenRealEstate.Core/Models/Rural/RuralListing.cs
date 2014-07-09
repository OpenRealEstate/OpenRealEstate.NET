using System;

namespace OpenRealEstate.Core.Models.Rural
{
    public class RuralListing : Listing
    {
        public CategoryType CategoryType { get; set; }
        public SalePricing Pricing { get; set; }
        public DateTime? AuctionOn { get; set; }
    }
}