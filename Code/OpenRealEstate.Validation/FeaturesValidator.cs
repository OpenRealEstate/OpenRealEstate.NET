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
            RuleFor(feature => feature.CarSpaces).GreaterThanOrEqualTo(0);
        }
    }
}