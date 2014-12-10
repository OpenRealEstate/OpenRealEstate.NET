using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class BuildingDetailsValidator : AbstractValidator<BuildingDetails>
    {
        public BuildingDetailsValidator()
        {
            RuleFor(building => building.Area).SetValidator(new UnitOfMeasureValidator());
            RuleFor(building => building.EnergyRating).GreaterThan(0).LessThanOrEqualTo(10);
        }
    }
}