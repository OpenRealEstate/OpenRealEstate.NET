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
        public static ValidationResult Validate(Listing listing, bool isTheMinimumDataToStoreAListing = true)
        {
            if (listing is ResidentialListing)
            {
                var validator = new ResidentialListingValidator();
                return isTheMinimumDataToStoreAListing
                    ? validator.Validate(listing as ResidentialListing, ruleSet: ResidentialListingValidator.MinimumRuleSet)
                    : validator.Validate(listing as ResidentialListing);
            }

            if (listing is RentalListing)
            {
                var validator = new RentalListingValidator();
                return isTheMinimumDataToStoreAListing
                    ? validator.Validate(listing as RentalListing, ruleSet: RentalListingValidator.MinimumRuleSet)
                    : validator.Validate(listing as RentalListing);
            }

            if (listing is RuralListing)
            {
                var validator = new RuralListingValidator();
                return isTheMinimumDataToStoreAListing
                    ? validator.Validate(listing as RuralListing, ruleSet: RuralListingValidator.MinimumRuleSet)
                    : validator.Validate(listing as RuralListing);;
            }

            if (listing is LandListing)
            {
                var validator = new LandListingValidator();
                return isTheMinimumDataToStoreAListing
                    ? validator.Validate(listing as LandListing, ruleSet: LandListingValidator.MinimumRuleSet)
                    : validator.Validate(listing as LandListing);
            }

            var errorMessage =
                string.Format(
                    "Tried to validate an unhandled Listing type: {0}. Only Residental, Rental, Rural and Land listing types are supported.",
                    listing.GetType());
            throw new Exception(errorMessage);
        }
    }
}