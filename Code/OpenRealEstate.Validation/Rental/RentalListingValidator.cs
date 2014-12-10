using System;
using FluentValidation;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Rental;

namespace OpenRealEstate.Validation.Rental
{
    public class RentalListingValidator : ListingValidator<RentalListing>
    {
        public RentalListingValidator()
        {
            // Optional.
            RuleFor(listing => listing.AvailableOn).NotEqual(DateTime.MinValue);
            RuleFor(listing => listing.BuildingDetails).SetValidator(new BuildingDetailsValidator());

            RuleSet(MinimumRuleSetKey, () =>
            {
                // Required.
                RuleFor(listing => listing.PropertyType).NotEqual(PropertyType.Unknown);
            });
        }
    }
}