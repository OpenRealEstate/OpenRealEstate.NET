using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Validation;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class AddressValidatorFacts
    {
        private readonly AddressValidator _addressValidator;

        public AddressValidatorFacts()
        {
            _addressValidator = new AddressValidator();    
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
            var address = new Address{ StreetNumber = "a" };

            // Act & Assert.
            _addressValidator.ShouldHaveValidationErrorFor(a => a.Street, address);
        }

        [Fact]
        public void GivenASuburb_Validate_ShouldNotHaveAValidationError()
        {
            _addressValidator.ShouldNotHaveValidationErrorFor(address => address.Suburb, "a");
        }

        [Fact]
        public void GivenNoSuburb_Validate_ShouldNHaveAValidationError()
        {
            _addressValidator.ShouldHaveValidationErrorFor(address => address.Suburb, "");
        }
    }
}