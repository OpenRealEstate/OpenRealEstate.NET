using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Validation;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class LandDetailsValidatorTests
    {
        [Fact]
        public void GivenAnArea_Validate_ShouldNotHaveAValidationError()
        {
            // Arrange.
            var validator = new LandDetailsValidator();
            var landDetails = new LandDetails
            {
                Area = new UnitOfMeasure
                {
                    Type = "a",
                    Value = 1m
                }
            };

            // Act.
            validator.ShouldHaveChildValidator(land => land.Area, typeof (UnitOfMeasureValidator));
            var result = validator.Validate(landDetails);
            //validator.ShouldHaveValidationErrorFor(land => land.Area, area);

            // Assert.
            result.Errors.Count.ShouldBe(0);
        }

        [Fact]
        public void GivenAnAreaWithNoType_Validate_ShouldHaveAValidationError()
        {
            // Arrange.
            var validator = new LandDetailsValidator();
            var landDetails = new LandDetails
            {
                Area = new UnitOfMeasure
                {
                    Type = null,
                    Value = 1m
                }
            };

            // Act.
            validator.ShouldHaveChildValidator(land => land.Area, typeof (UnitOfMeasureValidator));
            var result = validator.Validate(landDetails);
            //validator.ShouldHaveValidationErrorFor(land => land.Area, area);

            // Assert.
            result.Errors.ShouldContain(x => x.PropertyName == "Area.Type");
        }
    }
}