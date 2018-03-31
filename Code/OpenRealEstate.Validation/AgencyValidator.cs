using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class OfficeValidator : AggregateRootValidator<Offices>
    {
        public OfficeValidator()
        {
            RuleFor(Office => Office.FranchiseId).NotEmpty()
                .WithMessage(
                    "A FranchiseId is required. An Office has to be part of a franchise (including sole Offices).");

            RuleFor(Office => Office.Name).NotEmpty();
        }
    }
}