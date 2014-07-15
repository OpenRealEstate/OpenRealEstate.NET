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
        public void GivenAValidCarspaces_Validate_ShouldNotHaveAnError()
        {
            _featuresValidator.ShouldNotHaveValidationErrorFor(feature => feature.CarSpaces, 0);
        }

        [Fact]
        public void GivenAInvalidCarspaces_Validate_ShouldHaveAnError()
        {
            _featuresValidator.ShouldHaveValidationErrorFor(feature => feature.CarSpaces, -1);
        }
    }
}