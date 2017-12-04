using OpenRealEstate.Core;
using OpenRealEstate.Validation;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class CarParkingValidatorTests
    {
        private readonly CarParkingValidator _carParkingValidator;

        public CarParkingValidatorTests()
        {
            _carParkingValidator = new CarParkingValidator();
        }

        [Fact]
        public void GivenTheSumOfAllThreeCarparkingValuesWhichIsLessThan255_Validate_ShouldNotHaveAnError()
        {
            // Arrange.
            var carParking = new CarParking
            {
                Carports = 1,
                Garages = 2,
                OpenSpaces = 3
            };

            // Act.
            var result = _carParkingValidator.Validate(carParking);

            // Assert.
            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void GivenTheSumOfAllThreeCarparkingValuesWhichIsGreaterThan255_Validate_ShouldNotHaveAnError()
        {
            // Arrange.
            var carParking = new CarParking
            {
                Carports = 100,
                Garages = 100,
                OpenSpaces = 100
            };

            // Act.
            var result = _carParkingValidator.Validate(carParking);

            // Assert.
            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage.ShouldBe("The sum of Garages, Carports and Openspaces must not exceed 255. It is currently set at: 300.");
        }
    }
}