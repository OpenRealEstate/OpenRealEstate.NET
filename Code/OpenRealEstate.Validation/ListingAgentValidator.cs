using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class ListingAgentValidator : AbstractValidator<ListingAgent>
    {
        public ListingAgentValidator()
        {
            RuleFor(agent => agent.Name).NotEmpty();
            RuleFor(agent => agent.Communications).SetCollectionValidator(new CommunicationValidator());
        }
    }
}