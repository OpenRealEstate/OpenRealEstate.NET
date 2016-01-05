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
        public void GivenAFeature_SetValidator_ShouldHaveACarParkingValidator()
        {
            _featuresValidator.ShouldHaveChildValidator(feature => feature.CarParking, typeof(CarParkingValidator));
        }
    }
}