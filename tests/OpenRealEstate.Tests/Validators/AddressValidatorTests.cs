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

        [Theory]
        [InlineData("AU")]
        [InlineData("au")]
        public void GivenAValidCountryCode_Validate_ShouldNotHaveAValidationError(string countryCode)
        {
            _addressValidator.ShouldNotHaveValidationErrorFor(address => address.CountryIsoCode, countryCode);
        }

        [Theory]
        [InlineData("1234", "aa")]
        [InlineData("3000", "au")]
        [InlineData("3000", "AU")]
        public void GivenAValidPostcode_Validate_ShouldNotHaveAValidationError(string postcode,
                                                                               string countryCode)
        {
            var address = new Address
            {
                CountryIsoCode = countryCode,
                Postcode = postcode
            };
            _addressValidator.ShouldNotHaveValidationErrorFor(a => a.Postcode, address);
        }

        [Theory]
        [InlineData("0", "au")]
        [InlineData("100", "au")]
        [InlineData("10000", "AU")]
        [InlineData("", "AU")]
        [InlineData(null, "AU")]
        [InlineData("aaa", "AU")]
        public void GivenAnInvalidPostcode_Validate_ShouldHaveAValidationError(string postcode,
                                                                               string countryCode)
        {
            // Arrange.
            var address = new Address
            {
                CountryIsoCode = countryCode,
                Postcode = postcode
            };

            // Act & Assert.
            _addressValidator.ShouldHaveValidationErrorFor(a => a.Postcode, address);
        }

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("1234")]
        [InlineData("AbC")]
        public void GivenAnInvalidCountryCode_Validate_ShouldHaveAValidationError(string countryCode)
        {
            _addressValidator.ShouldHaveValidationErrorFor(address => address.CountryIsoCode, countryCode);
        }

        [Theory]
        [InlineData(123)]
        [InlineData(-123)]
        public void GivenAnInvalidLatitude_Validate_ShouldHaveAValidationError(decimal latitude)
        {
            _addressValidator.ShouldHaveValidationErrorFor(address => address.Latitude, latitude);
        }

        [Theory]
        [InlineData(181)]
        [InlineData(-181)]
        public void GivenAnInvalidLongitude_Validate_ShouldHaveAValidationError(decimal longitude)
        {
            _addressValidator.ShouldHaveValidationErrorFor(address => address.Longitude, longitude);
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