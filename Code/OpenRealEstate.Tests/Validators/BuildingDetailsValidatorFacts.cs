using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Validation;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class BuildingDetailsValidatorFacts
    {
        private readonly BuildingDetailsValidator _buildingDetailsValidator;

        public BuildingDetailsValidatorFacts()
        {
            _buildingDetailsValidator = new BuildingDetailsValidator();    
        }

        [Fact]
        public void GivenAnArea_Validate_ShouldNotHaveAValidationError()
        {
            // Arrange.
            var buildingDetails = new BuildingDetails
            {
                Area = new UnitOfMeasure
                {
                    Type = "a",
                    Value = 1m
                }
            };

            // Act.
            _buildingDetailsValidator.ShouldHaveChildValidator(building => building.Area, typeof(UnitOfMeasureValidator));
            var errors = _buildingDetailsValidator.Validate(buildingDetails);

            // Assert.
            errors.Errors.Count.ShouldBe(0);
        }

        [Fact]
        public void GivenAnAreaWithAMissingType_Validate_ShouldNotHaveAValidationError()
        {
            // Arrange.
            var buildingDetails = new BuildingDetails
            {
                Area = new UnitOfMeasure
                {
                    Type = null,
                    Value = 1m
                }
            };

            // Act.
            _buildingDetailsValidator.ShouldHaveChildValidator(building => building.Area, typeof(UnitOfMeasureValidator));
            var errors = _buildingDetailsValidator.Validate(buildingDetails);

            // Assert.
            errors.Errors.Count.ShouldBe(1);
        }

        [Fact]
        public void GivenAnErergyRating_Validate_ShouldNotHaveAValidationError()
        {
            _buildingDetailsValidator.ShouldNotHaveValidationErrorFor(a => a.EnergyRating, 1);
        }

        [Fact]
        public void GivenAnErergyRatingGreaterThan10_Validate_ShouldNotHaveAValidationError()
        {
            _buildingDetailsValidator.ShouldHaveValidationErrorFor(a => a.EnergyRating, (decimal?)10.1);
        }
    }
}