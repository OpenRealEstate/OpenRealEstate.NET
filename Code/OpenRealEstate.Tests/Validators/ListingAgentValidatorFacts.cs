using System.Collections.Generic;
using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Validation;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class ListingAgentValidatorFacts
    {
        private readonly ListingAgentValidator _validator;

        public ListingAgentValidatorFacts()
        {
            _validator = new ListingAgentValidator();
        }

        [Fact]
        public void GivenAName_Validate_ShouldNotHaveAValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(agent => agent.Name, "a");
        }

        [Fact]
        public void GivenAnInvalidOpensOn_Validate_ShouldHaveAValidationError()
        {
            _validator.ShouldHaveValidationErrorFor(agent => agent.Name, "");
        }

        [Fact]
        public void GiveSomeCommunication_Validate_ShouldNotHaveAValidationError()
        {
            // Arrange.
            var listingAgent = new ListingAgent
            {
                Name = "a"
            };
            listingAgent.AddCommunications(new List<Communication>
            {
                new Communication
                {
                    CommunicationType = CommunicationType.Email,
                    Details = "a"
                }
            });

            // Act.
            _validator.ShouldHaveChildValidator(agent => agent.Communications, typeof (CommunicationValidator));
            var result = _validator.Validate(listingAgent);
            //_validator.ShouldNotHaveValidationErrorFor(agent => agent.Communications, new[] {communication});

            // Assert.
            result.Errors.Count.ShouldBe(0);
        }

        [Fact]
        public void GiveACommunicationWithAnUnknownType_Validate_ShouldHaveAValidationError()
        {
            // Arrange.
            var listingAgent = new ListingAgent
            {
                Name = "a"
            };
            listingAgent.AddCommunications(new List<Communication>
            {
                new Communication
                {
                    CommunicationType = CommunicationType.Unknown,
                    Details = "a"
                }
            });

            // Act.
            _validator.ShouldHaveChildValidator(agent => agent.Communications, typeof(CommunicationValidator));
            var result = _validator.Validate(listingAgent);
            //_validator.ShouldNotHaveValidationErrorFor(agent => agent.Communications, new[] {communication});

            // Assert.
            result.Errors.ShouldContain(x => x.PropertyName == "Communications[0].CommunicationType");
        }
    }
}