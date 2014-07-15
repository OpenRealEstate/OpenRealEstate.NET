using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class LandDetailsValidator : AbstractValidator<LandDetails>
    {
        public LandDetailsValidator()
        {
            RuleFor(land => land.Area).SetValidator(new UnitOfMeasureValidator());
            RuleFor(land => land.Frontage).SetValidator(new UnitOfMeasureValidator());
            RuleFor(land => land.Depth).SetValidator(new UnitOfMeasureValidator());
        }
    }
}