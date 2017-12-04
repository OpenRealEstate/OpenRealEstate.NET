using FluentValidation;
using OpenRealEstate.Core;

namespace OpenRealEstate.Validation
{
    public class FranchiseValidator : AggregateRootValidator<Franchise>
    {
        public FranchiseValidator()
        {
            RuleFor(franchise => franchise.Name).NotEmpty()
                .WithMessage("A name is required. eg. Blue Ocean Properties.");
        }
    }
}