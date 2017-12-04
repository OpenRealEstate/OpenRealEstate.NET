using FluentValidation;
using OpenRealEstate.Core;

namespace OpenRealEstate.Validation
{
    public class MediaValidator : AbstractValidator<Media>
    {
        public MediaValidator()
        {
            RuleFor(media => media.Url).NotEmpty();
            RuleFor(media => media.Order).NotEmpty().GreaterThan(0);
        }
    }
}