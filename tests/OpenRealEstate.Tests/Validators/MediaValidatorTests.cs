using FluentValidation.TestHelper;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class MediaValidatorTests
    {
        public MediaValidatorTests()
        {
            _mediaValidator = new MediaValidator();
        }

        private readonly MediaValidator _mediaValidator;

        [Fact]
        public void GivenANegativeOrder_Validate_ShouldHaveAValidationError()
        {
            _mediaValidator.ShouldHaveValidationErrorFor(media => media.Order, -1);
        }

        [Fact]
        public void GivenAnOrder_Validate_ShouldNotHaveAValidationError()
        {
            _mediaValidator.ShouldNotHaveValidationErrorFor(media => media.Order, 1);
        }

        [Fact]
        public void GivenAUrl_Validate_ShouldNotHaveAValidationError()
        {
            _mediaValidator.ShouldNotHaveValidationErrorFor(media => media.Url, "a");
        }

        [Fact]
        public void GivenNoOrder_Validate_ShouldHaveAValidationError()
        {
            _mediaValidator.ShouldHaveValidationErrorFor(media => media.Order, 0);
        }

        [Fact]
        public void GivenNoUrl_Validate_ShouldHaveAValidationError()
        {
            _mediaValidator.ShouldHaveValidationErrorFor(media => media.Url, "");
        }
    }
}