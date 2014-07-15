using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class AgencyValidator : AggregateRootValidator<Agency>
    {
        public AgencyValidator()
        {
            RuleFor(agency => agency.FranchiseId).NotEmpty()
                .WithMessage(
                    "A FranchiseId is required. An Agency has to be part of a franchise (including sole agencies).");

            RuleFor(agency => agency.Name).NotEmpty();
        }
    }
}