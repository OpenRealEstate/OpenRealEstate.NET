using System;

namespace OpenRealEstate.Core.Models.Rental
{
    public class RentalListing : Listing
    {
        public PropertyType PropertyType { get; set; }
        public DateTime? AvailableOn { get; set; }
        public RentalPricing Pricing { get; set; }
    }
}