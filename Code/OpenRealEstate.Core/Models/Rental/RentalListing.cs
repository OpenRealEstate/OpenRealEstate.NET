using System;

namespace OpenRealEstate.Core.Models.Rental
{
    public class RentalListing : Listing
    {
        public PropertyType PropertyType { get; set; }
        public DateTime? AvailableOn { get; set; }
        public RentalPricing Pricing { get; set; }
        public BuildingDetails BuildingDetails { get; set; }

        public override string ToString()
        {
            return string.Format("Rental >> {0}", base.ToString());
        }
    }
}