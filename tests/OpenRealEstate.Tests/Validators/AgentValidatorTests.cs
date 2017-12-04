using FluentValidation.TestHelper;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class AgentValidatorTests
    {
        public AgentValidatorTests()
        {
            _agentValidator = new AgentValidator();
        }

        private readonly AgentValidator _agentValidator;

        [Fact]
        public void GivenAName_Validate_ShouldNotHaveAnError()
        {
            _agentValidator.ShouldNotHaveValidationErrorFor(agent => agent.Name, "a");
        }

        [Fact]
        public void GivenAtLeastOneAgencyId_Validate_ShouldNotHaveAnError()
        {
            // Arrange.
            var agencyIds = new[]
            {
                "a"
            };

            // Act & Assert.
            _agentValidator.ShouldNotHaveValidationErrorFor(agent => agent.AgencyIds, agencyIds);
        }

        [Fact]
        public void GivenNoAgencyIds_Validate_ShouldHaveAnError()
        {
            // Arrange.
            var agencyIds = new string[]
            {
            }; // No Agency Id's.

            // Act & Assert.
            _agentValidator.ShouldHaveValidationErrorFor(agent => agent.AgencyIds, agencyIds);
        }

        [Fact]
        public void GivenNoName_Validate_ShouldHaveAnError()
        {
            _agentValidator.ShouldHaveValidationErrorFor(agent => agent.Name, "");
        }
    }
}