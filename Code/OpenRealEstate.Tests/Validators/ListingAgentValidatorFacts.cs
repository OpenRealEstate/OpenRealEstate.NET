using System;
using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class ListingAgentValidatorFacts
    {
        private readonly ListingAgentValidator _listingAgentValidator;

        public ListingAgentValidatorFacts()
        {
            _listingAgentValidator = new ListingAgentValidator();
        }

        [Fact]
        public void GivenAName_Validate_ShouldNotHaveAValidationError()
        {
            _listingAgentValidator.ShouldNotHaveValidationErrorFor(agent => agent.Name, "a");
        }

        [Fact]
        public void GivenAnInvalidOpensOn_Validate_ShouldHaveAValidationError()
        {
            _listingAgentValidator.ShouldHaveValidationErrorFor(agent => agent.Name, "");
        }

        [Fact]
        public void GiveSomeCommunication_Validate_ShouldNotHaveAValidationError()
        {
            // Arrange.
            var communication = new Communication
            {
                CommunicationType = CommunicationType.Email,
                Details = "a"
            };

            // Act & Assert.
            _listingAgentValidator.ShouldHaveChildValidator(agent => agent.Communications, typeof(CommunicationValidator));
            _listingAgentValidator.ShouldNotHaveValidationErrorFor(agent => agent.Communications, new [] { communication} );
        }

        [Fact]
        public void GiveACommunicationWithAnUnknownType_Validate_ShouldHaveAValidationError()
        {
            // Arrange.
            var communication = new Communication
            {
                CommunicationType = CommunicationType.Unknown,
                Details = "a"
            };

            // Act & Assert.
            _listingAgentValidator.ShouldHaveChildValidator(agent => agent.Communications, typeof(CommunicationValidator));
            _listingAgentValidator.ShouldHaveValidationErrorFor(agent => agent.Communications, new[] { communication });
        }
    }
}