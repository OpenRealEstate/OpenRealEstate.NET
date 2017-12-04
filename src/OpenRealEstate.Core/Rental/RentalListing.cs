using System;

namespace OpenRealEstate.Core.Rental
{
    public class RentalListing : Listing, IPropertyType, IBuildingDetails
    {
        public override string ListingType => "Rental";

        public DateTime? AvailableOn { get; set; }

        public RentalPricing Pricing { get; set; }

        public BuildingDetails BuildingDetails { get; set; }

        public PropertyType PropertyType { get; set; }

        public override string ToString()
        {
            return $"Rental >> {base.ToString()}";
        }
    }
}