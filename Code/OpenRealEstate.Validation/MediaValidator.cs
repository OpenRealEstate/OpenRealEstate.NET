using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class MediaValidator : AbstractValidator<Media>
    {
        public MediaValidator()
        {
            RuleFor(media => media.Url).NotEmpty();
        }
    }
}