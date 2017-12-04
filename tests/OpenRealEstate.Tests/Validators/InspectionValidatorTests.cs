using System;
using FluentValidation.TestHelper;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class InspectionValidatorTests
    {
        public InspectionValidatorTests()
        {
            _inspectionValidator = new InspectionValidator();
        }

        private readonly InspectionValidator _inspectionValidator;

        [Fact]
        public void GiveAClosesOn_Validate_ShouldNotHaveAValidationError()
        {
            _inspectionValidator.ShouldNotHaveValidationErrorFor(inspection => inspection.ClosesOn, DateTime.UtcNow);
        }

        [Fact]
        public void GiveAnInvalidClosesOn_Validate_ShouldHaveAValidationError()
        {
            _inspectionValidator.ShouldHaveValidationErrorFor(inspection => inspection.ClosesOn, DateTime.MinValue);
        }

        [Fact]
        public void GivenAnInvalidOpensOn_Validate_ShouldHaveAValidationError()
        {
            _inspectionValidator.ShouldHaveValidationErrorFor(inspection => inspection.OpensOn, DateTime.MinValue);
        }

        [Fact]
        public void GivenAnOpensOn_Validate_ShouldNotHaveAValidationError()
        {
            _inspectionValidator.ShouldNotHaveValidationErrorFor(inspection => inspection.OpensOn, DateTime.UtcNow);
        }

        [Fact]
        public void GiveNoClosesOn_Validate_ShouldNotHaveAValidationError()
        {
            _inspectionValidator.ShouldNotHaveValidationErrorFor(inspection => inspection.ClosesOn, (DateTime?) null);
        }
    }
}