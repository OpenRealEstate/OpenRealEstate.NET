using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class AgentValidator : AggregateRootValidator<Agent>
    {
        public AgentValidator()
        {
            RuleFor(agent => agent.Name).NotEmpty()
                .WithMessage("A name is required. eg. Jane Smith.");

            RuleFor(agent => agent.AgencyIds).NotEmpty()
                .WithMessage("At least one AgencyId is requires where this Agent works at.");
        }
    }
}