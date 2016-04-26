using System;
using FluentValidation;
using OpenRealEstate.Core;

namespace OpenRealEstate.Validation
{
    public class AggregateRootValidator<T> : AbstractValidator<T> where  T : AggregateRoot
    {
        public AggregateRootValidator()
        {
            RuleFor(aggregateRoot => aggregateRoot.Id).NotEmpty()
                .WithMessage("An 'Id' is required. eg. RayWhite.Kew, Belle.Mosman69, 12345XXAbCdE");

            RuleFor(aggregateRoot => aggregateRoot.UpdatedOn).NotEqual(DateTime.MinValue)
                .WithMessage("A valid 'UpdatedOn' is required. Please use a date/time value that is in this decade or so.");
        }
    }
}