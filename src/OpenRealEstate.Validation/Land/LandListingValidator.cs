using System;
using FluentValidation;
using OpenRealEstate.Core.Land;

namespace OpenRealEstate.Validation.Land
{
    public class LandListingValidator : ListingValidator<LandListing>
    {
        public LandListingValidator()
        {
            // Can have a NULL AuctionOn date. Just can't have a MinValue one.
            RuleFor(listing => listing.AuctionOn).NotEqual(DateTime.MinValue);

            // Can have NULL Pricing. But if it's not NULL, then check it.
            RuleFor(listing => listing.Pricing).SetValidator(new SalePricingValidator());

            // NOTE: No rules needed for listing.LandEstate.
        }
    }
}