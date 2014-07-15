using System;
using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Rural;
using OpenRealEstate.Validation;
using OpenRealEstate.Validation.Rural;
using Xunit;

namespace OpenRealEstate.Tests.Validators.Rural
{
    public class RuralListingValidatorFacts
    {
        private readonly RuralListingValidator _validator;

        public RuralListingValidatorFacts()
        {
            _validator = new RuralListingValidator();
        }

        [Fact]
        public void GivenACategoryType_Validate_ShouldNotHaveAValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(listing => listing.CategoryType, CategoryType.Cropping);
        }

        [Fact]
        public void GivenAnUnknownCategoryType_Validate_ShouldHaveAValidationError()
        {
            _validator.ShouldHaveValidationErrorFor(listing => listing.CategoryType, CategoryType.Unknown);
        }

        [Fact]
        public void GivenAnAuctionOn_Validate_ShouldNotHaveAValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(listing => listing.AuctionOn, DateTime.UtcNow);
        }

        [Fact]
        public void GivenAnInvalidAuctionOn_Validate_ShouldHaveAValidationError()
        {
            _validator.ShouldHaveValidationErrorFor(listing => listing.AuctionOn, DateTime.MinValue);
        }

        [Fact]
        public void GivenANullAuctionOn_Validate_ShouldNotHaveAValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(listing => listing.AuctionOn, (DateTime?) null);
        }

        [Fact]
        public void GivenASalePricing_Validate_ShouldNotHaveAValidationError()
        {
            // Arrange.
            var salePricing = new SalePricing
            {
                SalePrice = 1234,
                SalePriceText = "Contact agent"
            };

            // Act & Assert.
            _validator.ShouldHaveChildValidator(listing => listing.Pricing, typeof(SalePricingValidator));
            _validator.ShouldNotHaveValidationErrorFor(listing => listing.Pricing, salePricing);
        }
    }
}
