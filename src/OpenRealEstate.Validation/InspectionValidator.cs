using System;
using FluentValidation;
using OpenRealEstate.Core;

namespace OpenRealEstate.Validation
{
    public class InspectionValidator : AbstractValidator<Inspection>
    {
        public InspectionValidator()
        {
            RuleFor(inspection => inspection.OpensOn).NotEqual(DateTime.MinValue)
                .WithMessage(
                    "The Date/Time value is illegal. Please use a valid value, which is a more current value .. like .. something from this century, please.");

            RuleFor(inspection => inspection.ClosesOn).NotEqual(DateTime.MinValue)
                .When(inspection => inspection.ClosesOn.HasValue)
                .WithMessage(
                    "The Date/Time value is illegal. Please use a valid value, which is a more current value .. like .. something from this century, please, or a NULL value (ie. Not sure when it closes on).");
        }
    }
}