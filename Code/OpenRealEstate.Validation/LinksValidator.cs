using System;
using FluentValidation;

namespace OpenRealEstate.Validation
{
    public class LinksValidator : AbstractValidator<string>
    {
        public LinksValidator()
        {
            RuleFor(x => x)
                .Must(LinkMustBeAUri)
                .WithMessage("Link '{PropertyValue}' must be a valid URI. eg: http://www.SomeWebSite.com.au");
        }

        private static bool LinkMustBeAUri(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return false;
            }

            Uri result;
            return Uri.TryCreate(link, UriKind.Absolute, out result);
        }
    }
}