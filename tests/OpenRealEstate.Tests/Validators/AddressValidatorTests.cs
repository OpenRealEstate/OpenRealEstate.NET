using FluentValidation.TestHelper;
using OpenRealEstate.Core;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class AddressValidatorTests
    {
        public AddressValidatorTests()
        {
            _addressValidator = new AddressValidator();
        }

        private readonly AddressValidator _addressValidator;

        [Fact]
        public void GivenALatitude_Validate_ShouldNotHaveAValidationError()
        {
            _addressValidator.ShouldNotHaveValidationErrorFor(address => address.Latitude, -23M);
        }

        [Fact]
        public void GivenALongitude_Validate_ShouldNotHaveAValidationError()
        {
            _addressValidator.ShouldNotHaveValidationErrorFor(address => address.Longitude, 170M);
        }

        [Fact]
        public void GivenAnInvalidLatitude_Validate_ShouldHaveAValidationError()
        {
            _addressValidator.ShouldHaveValidationErrorFor(address => address.Latitude, -123M);
        }

        [Fact]
        public void GivenAnInvalidLongitude_Validate_ShouldHaveAValidationError()
        {
            _addressValidator.ShouldHaveValidationErrorFor(address => address.Longitude, 200M);
        }

        [Fact]
        public void GivenAStreetNumberAndAStreetName_Validate_ShouldNotHaveAValidationError()
        {
            // Arrange.
            var address = new Address
            {
                StreetNumber = "a",
                Street = "b"
            };

            // Act & Assert.
            _addressValidator.ShouldNotHaveValidationErrorFor(a => a.Street, address);
        }

        [Fact]
        public void GivenAStreetNumberAndNoStreetName_Validate_ShouldHaveAValidationError()
        {
            // Arrange.
            var address = new Address
            {
                StreetNumber = "a"
            };

            // Act & Assert.
            _addressValidator.ShouldHaveValidationErrorFor(a => a.Street, address);
        }

        [Fact]
        public void GivenASuburb_Validate_ShouldNotHaveAValidationError()
        {
            _addressValidator.ShouldNotHaveValidationErrorFor(address => address.Suburb, "a");
        }

        [Fact]
        public void GivenNoLatitude_Validate_ShouldNotHaveAValidationError()
        {
            _addressValidator.ShouldNotHaveValidationErrorFor(address => address.Latitude, (decimal?) null);
        }

        [Fact]
        public void GivenNoLongitude_Validate_ShouldNotHaveAValidationError()
        {
            _addressValidator.ShouldNotHaveValidationErrorFor(address => address.Longitude, (decimal?) null);
        }

        [Fact]
        public void GivenNoSuburb_Validate_ShouldHaveAValidationError()
        {
            _addressValidator.ShouldHaveValidationErrorFor(address => address.Suburb, "");
        }
    }
}