using System;
using FluentValidation.TestHelper;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class SalePricingValidatorTests
    {
        private readonly SalePricingValidator _salePricingValidator;

        public SalePricingValidatorTests()
        {
            _salePricingValidator = new SalePricingValidator();
        }

        [Fact]
        public void GivenASalePrice_Validate_ShouldNotHaveAValidationError()
        {
            _salePricingValidator.ShouldNotHaveValidationErrorFor(salePricing => salePricing.SalePrice, 1);
        }

        [Fact]
        public void GivenANegativeSalePrice_Validate_ShouldHaveAValidationError()
        {
            _salePricingValidator.ShouldHaveValidationErrorFor(salePrice => salePrice.SalePrice, -1);
        }

        [Fact]
        public void GivenASoldPrice_Validate_ShouldNotHaveAValidationError()
        {
            _salePricingValidator.ShouldNotHaveValidationErrorFor(salePrice => salePrice.SoldPrice, 1);
        }

        [Fact]
        public void GivenANegativeSoldPrice_Validate_ShouldHaveAValidationError()
        {
            _salePricingValidator.ShouldHaveValidationErrorFor(salePrice => salePrice.SoldPrice, -1);
        }

        [Fact]
        public void GivenASoldOn_Validate_ShouldNotHaveAValidationError()
        {
            _salePricingValidator.ShouldNotHaveValidationErrorFor(salePrice => salePrice.SoldOn, DateTime.UtcNow);
        }

        [Fact]
        public void GivenNoSoldOn_Validate_ShouldNotHaveAValidationError()
        {
            _salePricingValidator.ShouldNotHaveValidationErrorFor(salePrice => salePrice.SoldOn, (DateTime?) null);
        }

        [Fact]
        public void GivenAnInvalidSoldOn_Validate_ShouldHaveAValidationError()
        {
            _salePricingValidator.ShouldHaveValidationErrorFor(salePrice => salePrice.SoldOn, DateTime.MinValue);
        }
    }
}