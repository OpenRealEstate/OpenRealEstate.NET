using FluentValidation;
using OpenRealEstate.Core;

namespace OpenRealEstate.Validation
{
    public class UnitOfMeasureValidator : AbstractValidator<UnitOfMeasure>
    {
        public UnitOfMeasureValidator()
        {
            RuleFor(unit => unit.Value).GreaterThanOrEqualTo(0);

            RuleFor(unit => unit.Type).NotNull()
                .When(unit => unit.Value > 0)
                .WithMessage("If a unit of measure's 'Value' is provided, then a 'Type' also needs to be provided.");
        }
    }
}