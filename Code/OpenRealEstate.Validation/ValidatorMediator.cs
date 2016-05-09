using System;
using FluentValidation;
using FluentValidation.Results;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Land;
using OpenRealEstate.Core.Rental;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Core.Rural;
using OpenRealEstate.Validation.Land;
using OpenRealEstate.Validation.Rental;
using OpenRealEstate.Validation.Residential;
using OpenRealEstate.Validation.Rural;

namespace OpenRealEstate.Validation
{
    public static class ValidatorMediator
    {
        public static ValidationResult Validate(Listing listing, ListingValidatorRuleSet ruleSet)
        {
            return Validate(listing, ruleSet.ToDescription());
        }
        
        public static ValidationResult Validate(Listing listing, string ruleSet = ResidentialListingValidator.NormalRuleSet)
        {
            if (listing == null)
            {
                throw new ArgumentNullException();
            }

            if (listing is ResidentialListing)
            {
                var validator = new ResidentialListingValidator();
                return string.IsNullOrWhiteSpace(ruleSet)
                    ? validator.Validate((ResidentialListing)listing)
                    : validator.Validate((ResidentialListing)listing, ruleSet: ruleSet);
            }

            if (listing is RentalListing)
            {
                var validator = new RentalListingValidator();
                return string.IsNullOrWhiteSpace(ruleSet)
                    ? validator.Validate((RentalListing) listing)
                    : validator.Validate((RentalListing) listing, ruleSet: ruleSet);
            }

            if (listing is RuralListing)
            {
                var validator = new RuralListingValidator();
                return string.IsNullOrWhiteSpace(ruleSet)
                    ? validator.Validate((RuralListing) listing)
                    : validator.Validate((RuralListing) listing, ruleSet: ruleSet);
            }

            if (listing is LandListing)
            {
                var validator = new LandListingValidator();
                return string.IsNullOrWhiteSpace(ruleSet)
                    ? validator.Validate((LandListing) listing)
                    : validator.Validate((LandListing) listing, ruleSet: ruleSet);
            }

            var errorMessage =
                $"Tried to validate an unhandled Listing type: {listing.GetType()}. Only Residental, Rental, Rural and Land listing types are supported.";
            throw new Exception(errorMessage);
        }
    }
}