using FluentValidation.TestHelper;
using OpenRealEstate.Validation.Rental;
using Xunit;

namespace OpenRealEstate.Tests.Validators.Rental
{
    public class RentalPricingValidatorFacts
    {
        private readonly RentalPricingValidator _validator;

        public RentalPricingValidatorFacts()
        {
            _validator = new RentalPricingValidator();
        }

        [Fact]
        public void GivenABond_Validate_ShouldNotHaveAValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(rentalPricing => rentalPricing.Bond, 1);
        }

        [Fact]
        public void GivenNoBond_Validate_ShouldNotHaveAValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(rentalPricing => rentalPricing.Bond, 0);
        }

        [Fact]
        public void GivenANegativeBond_Validate_ShouldHaveAValidationError()
        {
            _validator.ShouldHaveValidationErrorFor(rentalPricing => rentalPricing.Bond, -1);
        }

        [Fact]
        public void GivenARentalPrice_Validate_ShouldNotHaveAValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(rentalPricing => rentalPricing.RentalPrice, 1);
        }

        [Fact]
        public void GivenNoRentalPrice_Validate_ShouldNotHaveAValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(rentalPricing => rentalPricing.RentalPrice, 0);
        }

        [Fact]
        public void GivenANegativeRentalPrice_Validate_ShouldHaveAValidationError()
        {
            _validator.ShouldHaveValidationErrorFor(rentalPricing => rentalPricing.RentalPrice, -1);
        }
    }
}