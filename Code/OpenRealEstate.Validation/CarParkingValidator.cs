using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class CarParkingValidator : AbstractValidator<CarParking>
    {
        public CarParkingValidator()
        {
            RuleFor(carParking => carParking.Carports).GreaterThanOrEqualTo(0);
            RuleFor(carParking => carParking.Garages).GreaterThanOrEqualTo(0);
            RuleFor(carParking => carParking.OpenSpaces).GreaterThanOrEqualTo(0);
        }
    }
}