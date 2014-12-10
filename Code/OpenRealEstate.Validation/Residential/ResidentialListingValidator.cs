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
                
                RuleFor(listing => listing.Pricing).SetValidator(new SalePricingValidator());    
            });

            // Optional.
            RuleFor(listing => listing.AuctionOn).NotEqual(DateTime.MinValue);
            RuleFor(listing => listing.BuildingDetails).SetValidator(new BuildingDetailsValidator());
        }
    }
}