using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class FeaturesValidator : AbstractValidator<Features>
    {
        public FeaturesValidator()
        {
            RuleFor(feature => feature.Bedrooms).GreaterThanOrEqualTo(0);
            RuleFor(feature => feature.Bathrooms).GreaterThanOrEqualTo(0);
            RuleFor(feature => feature.Ensuites).GreaterThanOrEqualTo(0);
            RuleFor(feature => feature.CarParking).SetValidator(new CarParkingValidator());
            RuleFor(feature => feature.Toilets).GreaterThanOrEqualTo(0);
            RuleFor(feature => feature.LivingAreas).GreaterThanOrEqualTo(0);

            // NOTE: Tags can be null or contains a list of tags.
        }
    }
}