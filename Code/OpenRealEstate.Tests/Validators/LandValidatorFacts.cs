using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class LandValidatorFacts
    {
        private readonly LandDetailsValidator _landDetailsValidator;

        public LandValidatorFacts()
        {
            _landDetailsValidator = new LandDetailsValidator();
        }

        [Fact]
        public void GivenAnArea_Validate_ShouldNotHaveAValidationError()
        {
            // Arrange.
            var area = new UnitOfMeasure
            {
                Type = "a",
                Value = -1123213m
            };

            // Act & Assert.
            _landDetailsValidator.ShouldHaveChildValidator(land => land.Area, typeof(UnitOfMeasureValidator));
            _landDetailsValidator.ShouldNotHaveValidationErrorFor(land => land.Area, area);
        }

        [Fact]
        public void GivenAnAreaWithNoType_Validate_ShouldHaveAValidationError()
        {
            // Arrange.
            var area = new UnitOfMeasure
            {
                Type = null,
                Value = 1m
            };

            // Act & Assert.
            _landDetailsValidator.ShouldHaveChildValidator(land => land.Area, typeof(UnitOfMeasureValidator));
            _landDetailsValidator.ShouldHaveValidationErrorFor(land => land.Area, area);
        }
    }
}