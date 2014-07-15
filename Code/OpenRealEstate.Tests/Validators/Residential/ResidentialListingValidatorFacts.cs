using System;
using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Validation.Residential;
using Xunit;

namespace OpenRealEstate.Tests.Validators.Residential
{
    public class ResidentialListingValidatorFacts
    {
        private readonly ResidentialListingValidator _validator;

        public ResidentialListingValidatorFacts()
        {
            _validator = new ResidentialListingValidator();
        }

        [Fact]
        public void GivenAPropertyType_Validate_ShouldNotHaveAValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(listing => listing.PropertyType, PropertyType.Townhouse);
        }

        [Fact]
        public void GivenAnUnknownPropertyType_Validate_ShouldHaveAValidationError()
        {
            _validator.ShouldHaveValidationErrorFor(listing => listing.PropertyType, PropertyType.Unknown);
        }

        [Fact]
        public void GivenAnAuctionOn_Validate_ShouldNotHaveAValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(listing => listing.AuctionOn, DateTime.UtcNow);
        }

        [Fact]
        public void Sdfddf()
        {
            _validator.ShouldNotHaveValidationErrorFor(listing => listing.Title, (string)null);
        }
    }
}