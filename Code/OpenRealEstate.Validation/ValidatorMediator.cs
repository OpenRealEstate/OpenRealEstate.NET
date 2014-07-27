using System;
using FluentValidation;
using FluentValidation.Results;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;
using OpenRealEstate.Validation.Land;
using OpenRealEstate.Validation.Rental;
using OpenRealEstate.Validation.Residential;
using OpenRealEstate.Validation.Rural;

namespace OpenRealEstate.Validation
{
    public static class ValidatorMediator
    {
        public const string MinimumRuleSet = ListingValidator<ResidentialListing>.MinimumRuleSet;

        public static ValidationResult Validate(Listing listing, string ruleSet = null)
        {
            if (listing is ResidentialListing)
            {
                var validator = new ResidentialListingValidator();
                return string.IsNullOrWhiteSpace(ruleSet)
                    ? validator.Validate(listing as ResidentialListing)
                    : validator.Validate(listing as ResidentialListing, ruleSet: RentalListingValidator.MinimumRuleSet);
            }

            if (listing is RentalListing)
            {
                var validator = new RentalListingValidator();
                return string.IsNullOrWhiteSpace(ruleSet)
                    ? validator.Validate(listing as RentalListing)
                    : validator.Validate(listing as RentalListing, ruleSet: RentalListingValidator.MinimumRuleSet);
            }

            if (listing is RuralListing)
            {
                var validator = new RuralListingValidator();
                return string.IsNullOrWhiteSpace(ruleSet)
                    ? validator.Validate(listing as RuralListing)
                    : validator.Validate(listing as RuralListing, ruleSet: RentalListingValidator.MinimumRuleSet);;
            }

            if (listing is LandListing)
            {
                var validator = new LandListingValidator();
                return string.IsNullOrWhiteSpace(ruleSet)
                    ? validator.Validate(listing as LandListing)
                    : validator.Validate(listing as LandListing, ruleSet: RentalListingValidator.MinimumRuleSet);
            }

            var errorMessage =
                string.Format(
                    "Tried to validate an unhandled Listing type: {0}. Only Residental, Rental, Rural and Land listing types are supported.",
                    listing.GetType());
            throw new Exception(errorMessage);
        }
    }
}