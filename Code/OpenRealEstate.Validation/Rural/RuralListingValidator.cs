using System;
using FluentValidation;
using OpenRealEstate.Core.Rural;

namespace OpenRealEstate.Validation.Rural
{
    public class RuralListingValidator : ListingValidator<RuralListing>
    {
        public RuralListingValidator()
        {
            // Can have a NULL AuctionOn date. Just can't have a MinValue one.
            RuleFor(listing => listing.AuctionOn).NotEqual(DateTime.MinValue);

            // Can have NULL Pricing. But if it's not NULL, then check it.
            RuleFor(listing => listing.Pricing).SetValidator(new SalePricingValidator());

            // Can have NULL building details. But if it's not NULL, then check it.
            RuleFor(listing => listing.BuildingDetails).SetValidator(new BuildingDetailsValidator());

            RuleSet(NormalRuleSetKey, () => 
                RuleFor(listing => listing.CategoryType).NotEqual(CategoryType.Unknown));
        }
    }
}