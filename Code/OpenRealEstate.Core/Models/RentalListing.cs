using System;

namespace OpenRealEstate.Core.Models
{
    public class RentalListing : Listing
    {
        public DateTime? AvailableOn { get; set; }
        public RentalPricing Pricing { get; set; }
    }
}