using FluentValidation.TestHelper;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class MediaValidatorFacts
    {
        private readonly MediaValidator _mediaValidator;

        public MediaValidatorFacts()
        {
            _mediaValidator = new MediaValidator();
        }

        [Fact]
        public void GivenAUrl_Validate_ShouldNotHaveAValidationError()
        {
            _mediaValidator.ShouldNotHaveValidationErrorFor(media => media.Url, "a");
        }

        [Fact]
        public void GivenNoUrl_Validate_ShouldHaveAValidationError()
        {
            _mediaValidator.ShouldHaveValidationErrorFor(media => media.Url, "");
        }
    }
}