using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class CommunicationValidatorTests
    {
        private readonly CommunicationValidator _communicationValidator;

        public CommunicationValidatorTests()
        {
            _communicationValidator = new CommunicationValidator();
        }

        [Fact]
        public void GivenAValidCommunicationType_Validate_ShouldNotHaveAnError()
        {
            _communicationValidator.ShouldNotHaveValidationErrorFor(communication => communication.CommunicationType, CommunicationType.Email);
        }

        [Fact]
        public void GivenACommunicationUnknown_Validate_ShouldHaveAnError()
        {
            _communicationValidator.ShouldHaveValidationErrorFor(communication => communication.CommunicationType, CommunicationType.Unknown);
        }

        [Fact]
        public void GivenADetails_Validate_ShouldNotHaveAnError()
        {
            _communicationValidator.ShouldNotHaveValidationErrorFor(communication => communication.Details, "aa");
        }

        [Fact]
        public void GivenNoDetails_Validate_ShouldHaveAnError()
        {
            _communicationValidator.ShouldHaveValidationErrorFor(communication => communication.Details, "");
        }
    }
}