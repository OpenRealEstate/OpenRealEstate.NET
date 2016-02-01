using FluentValidation.TestHelper;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class FranchiseValidatorTests
    {
        private readonly FranchiseValidator _franchiseValidator;

        public FranchiseValidatorTests()
        {
            _franchiseValidator = new FranchiseValidator();
        }

        [Fact]
        public void GivenAnId_Validate_ShouldNotHaveAValidationError()
        {
            _franchiseValidator.ShouldNotHaveValidationErrorFor(franchise => franchise.Id, "a");
        }

        [Fact]
        public void GivenAnId_Validate_ShouldHaveAValidationError()
        {
            _franchiseValidator.ShouldHaveValidationErrorFor(franchise => franchise.Id, "");
        }
    }
}