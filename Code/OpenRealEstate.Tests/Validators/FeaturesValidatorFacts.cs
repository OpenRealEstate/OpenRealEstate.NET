using FluentValidation.TestHelper;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class FeaturesValidatorFacts
    {
        private readonly FeaturesValidator _featuresValidator;

        public FeaturesValidatorFacts()
        {
            _featuresValidator = new FeaturesValidator();
        }

        [Fact]
        public void GivenAValidBedroom_Validate_ShouldNotHaveAnError()
        {
            _featuresValidator.ShouldNotHaveValidationErrorFor(feature => feature.Bedrooms, 0);
        }

        [Fact]
        public void GivenAInvalidBedroom_Validate_ShouldHaveAnError()
        {
            _featuresValidator.ShouldHaveValidationErrorFor(feature => feature.Bedrooms, -1);
        }

        [Fact]
        public void GivenAValidBathroom_Validate_ShouldNotHaveAnError()
        {
            _featuresValidator.ShouldNotHaveValidationErrorFor(feature => feature.Bathrooms, 0);
        }

        [Fact]
        public void GivenAInvalidBathroom_Validate_ShouldHaveAnError()
        {
            _featuresValidator.ShouldHaveValidationErrorFor(feature => feature.Bathrooms, -1);
        }

        [Fact]
        public void GivenAValidToilet_Validate_ShouldNotHaveAnError()
        {
            _featuresValidator.ShouldNotHaveValidationErrorFor(feature => feature.Toilets, 0);
        }

        [Fact]
        public void GivenAInvalidToilet_Validate_ShouldHaveAnError()
        {
            _featuresValidator.ShouldHaveValidationErrorFor(feature => feature.Toilets, -1);
        }

        [Fact]
        public void GivenAValidGarage_Validate_ShouldNotHaveAnError()
        {
            _featuresValidator.ShouldNotHaveValidationErrorFor(feature => feature.Garages, 0);
        }

        [Fact]
        public void GivenAInvalidGarage_Validate_ShouldHaveAnError()
        {
            _featuresValidator.ShouldHaveValidationErrorFor(feature => feature.Garages, -1);
        }

        [Fact]
        public void GivenAValidCarport_Validate_ShouldNotHaveAnError()
        {
            _featuresValidator.ShouldNotHaveValidationErrorFor(feature => feature.Carports, 0);
        }

        [Fact]
        public void GivenAInvalidCarport_Validate_ShouldHaveAnError()
        {
            _featuresValidator.ShouldHaveValidationErrorFor(feature => feature.Carports, -1);
        }

        [Fact]
        public void GivenAValidLivingArea_Validate_ShouldNotHaveAnError()
        {
            _featuresValidator.ShouldNotHaveValidationErrorFor(feature => feature.LivingAreas, 0);
        }

        [Fact]
        public void GivenAInvalidLivingArea_Validate_ShouldHaveAnError()
        {
            _featuresValidator.ShouldHaveValidationErrorFor(feature => feature.LivingAreas, -1);
        }

        [Fact]
        public void GivenAValidOpenSpace_Validate_ShouldNotHaveAnError()
        {
            _featuresValidator.ShouldNotHaveValidationErrorFor(feature => feature.OpenSpaces, 0);
        }

        [Fact]
        public void GivenAInvalidOpenSpace_Validate_ShouldHaveAnError()
        {
            _featuresValidator.ShouldHaveValidationErrorFor(feature => feature.OpenSpaces, -1);
        }
    }
}