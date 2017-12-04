using System;
using FluentValidation.TestHelper;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class AgencyValidatorTests
    {
        public AgencyValidatorTests()
        {
            _agencyValidator = new AgencyValidator();
        }

        private readonly AgencyValidator _agencyValidator;

        [Fact]
        public void GivenAFranchiseId_Validate_ShouldNotHaveAValidationError()
        {
            _agencyValidator.ShouldNotHaveValidationErrorFor(agency => agency.FranchiseId, "a");
        }

        [Fact]
        public void GivenAName_Validate_ShouldNotHaveAValidationError()
        {
            _agencyValidator.ShouldNotHaveValidationErrorFor(agency => agency.Name, "a");
        }

        [Fact]
        public void GivenAnId_Validate_ShouldNotHaveAValidationError()
        {
            _agencyValidator.ShouldNotHaveValidationErrorFor(agency => agency.Id, "a");
        }

        [Fact]
        public void GivenAnInvalidUpdatedOn_Validate_ShouldHaveAValidationError()
        {
            _agencyValidator.ShouldHaveValidationErrorFor(agency => agency.UpdatedOn, DateTime.MinValue);
        }

        [Fact]
        public void GivenAValidUpdatedOn_Validate_ShouldNotHaveAValidationError()
        {
            _agencyValidator.ShouldNotHaveValidationErrorFor(agency => agency.UpdatedOn, DateTime.UtcNow);
        }

        [Fact]
        public void GivenNoFranchiseId_Validate_ShouldHaveAValidationError()
        {
            _agencyValidator.ShouldHaveValidationErrorFor(agency => agency.FranchiseId, "");
        }

        [Fact]
        public void GivenNoId_Validate_ShouldHaveAValidationError()
        {
            _agencyValidator.ShouldHaveValidationErrorFor(agency => agency.Id, "");
        }

        [Fact]
        public void GivenNoName_Validate_ShouldHaveAValidationError()
        {
            _agencyValidator.ShouldHaveValidationErrorFor(agency => agency.Name, "");
        }
    }
}