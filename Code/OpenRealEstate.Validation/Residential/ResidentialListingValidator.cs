using System;
using FluentValidation;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Residential;

namespace OpenRealEstate.Validation.Residential
{
    public class ResidentialListingValidator : ListingValidator<ResidentialListing>
    {
        public ResidentialListingValidator()
        {
            RuleSet(MinimumRuleSetKey, () =>
            {
                // Required.
                RuleFor(listing => listing.PropertyType).NotEqual(PropertyType.Unknown)
                    .WithMessage("Invalid 'PropertyType'. Please choose any property except Unknown.");
                
                // Optional.
                RuleFor(listing => listing.Pricing).SetValidator(new SalePricingValidator());
                RuleFor(listing => listing.AuctionOn).NotEqual(DateTime.MinValue);
            });
        }
    }
}