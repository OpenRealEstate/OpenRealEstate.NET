using FluentValidation.TestHelper;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class CarParkingValidatorFacts
    {
        private readonly CarParkingValidator _carParkingValidator;

        public CarParkingValidatorFacts()
        {
            _carParkingValidator = new CarParkingValidator();
        }

        [Fact]
        public void GivenAValidGarage_Validate_ShouldNotHaveAnError()
        {
            _carParkingValidator.ShouldNotHaveValidationErrorFor(feature => feature.Garages, 0);
        }

        [Fact]
        public void GivenAInvalidGarage_Validate_ShouldHaveAnError()
        {
            _carParkingValidator.ShouldHaveValidationErrorFor(feature => feature.Garages, -1);
        }

        [Fact]
        public void GivenAValidCarport_Validate_ShouldNotHaveAnError()
        {
            _carParkingValidator.ShouldNotHaveValidationErrorFor(feature => feature.Carports, 0);
        }

        [Fact]
        public void GivenAInvalidCarport_Validate_ShouldHaveAnError()
        {
            _carParkingValidator.ShouldHaveValidationErrorFor(feature => feature.Carports, -1);
        }

        [Fact]
        public void GivenAValidOpenSpaces_Validate_ShouldNotHaveAnError()
        {
            _carParkingValidator.ShouldNotHaveValidationErrorFor(feature => feature.OpenSpaces, 0);
        }

        [Fact]
        public void GivenAInvalidOpenSpaces_Validate_ShouldHaveAnError()
        {
            _carParkingValidator.ShouldHaveValidationErrorFor(feature => feature.OpenSpaces, -1);
        }
    }
}