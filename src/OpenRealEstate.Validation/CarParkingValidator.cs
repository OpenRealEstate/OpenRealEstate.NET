using FluentValidation;
using OpenRealEstate.Core;

namespace OpenRealEstate.Validation
{
    public class CarParkingValidator : AbstractValidator<CarParking>
    {
        public CarParkingValidator()
        {
            RuleFor(carParking => carParking.Carports + carParking.Garages + carParking.OpenSpaces)
                .LessThanOrEqualTo(byte.MaxValue)
                .WithMessage("The sum of Garages, Carports and Openspaces must not exceed 255. It is currently set at: {PropertyValue}.");

            // NOTE: Byte properties are from 0->255, so they don't need a validator check.
        }
    }
}