using System;
using FluentValidation;
using OpenRealEstate.Core.Models.Rural;

namespace OpenRealEstate.Validation.Rural
{
    public class RuralListingValidator : ListingValidator<RuralListing>
    {
        public RuralListingValidator()
        {
            RuleFor(listing => listing.CategoryType).NotEqual(CategoryType.Unknown);
            RuleFor(listing => listing.AuctionOn).NotEqual(DateTime.MinValue);
            RuleFor(listing => listing.Pricing).SetValidator(new SalePricingValidator());
        }
    }
}