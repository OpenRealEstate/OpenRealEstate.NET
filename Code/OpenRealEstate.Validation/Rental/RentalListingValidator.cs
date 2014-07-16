using System;
using FluentValidation;
using OpenRealEstate.Core.Models.Rental;

namespace OpenRealEstate.Validation.Rental
{
    public class RentalListingValidator : ListingValidator<RentalListing>
    {
        public RentalListingValidator()
        {
            // Optional.
            RuleFor(listing => listing.AvailableOn).NotEqual(DateTime.MinValue);
        }
    }
}