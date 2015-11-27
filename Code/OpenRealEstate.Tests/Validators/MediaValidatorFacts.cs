using System;
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

        [Fact]
        public void GivenAnOrder_Validate_ShouldNotHaveAValidationError()
        {
            _mediaValidator.ShouldNotHaveValidationErrorFor(media => media.Order, 1);
        }

        [Fact]
        public void GivenNoOrder_Validate_ShouldHaveAValidationError()
        {
            _mediaValidator.ShouldHaveValidationErrorFor(media => media.Order, 0);
        }

        [Fact]
        public void GivenANegativeOrder_Validate_ShouldHaveAValidationError()
        {
            _mediaValidator.ShouldHaveValidationErrorFor(media => media.Order, -1);
        }

        [Fact]
        public void GivenACreatedOn_Validate_ShouldNotHaveAValidationError()
        {
            _mediaValidator.ShouldNotHaveValidationErrorFor(media => media.CreatedOn, DateTime.UtcNow);
        }

        [Fact]
        public void GivenNoCreatedOn_Validate_ShouldHaveAValidationError()
        {
            _mediaValidator.ShouldHaveValidationErrorFor(media => media.CreatedOn, DateTime.MinValue);
        }
    }
}