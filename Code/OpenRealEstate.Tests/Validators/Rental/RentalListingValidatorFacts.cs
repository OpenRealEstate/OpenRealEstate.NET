using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using OpenRealEstate.Validation.Rental;
using Xunit;

namespace OpenRealEstate.Tests.Validators.Rental
{
    public class RentalListingValidatorFacts
    {
        private readonly RentalListingValidator _validator;

        public RentalListingValidatorFacts()
        {
            _validator = new RentalListingValidator();
        }

        [Fact]
        public void GivenAnAvailableOn_Validate_ShouldNotHaveAValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(listing => listing.AvailableOn, DateTime.UtcNow);
        }

        [Fact]
        public void GivenNoAvailableOn_Validate_ShouldNotHaveAValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(listing => listing.AvailableOn, (DateTime?)null);
        }

        [Fact]
        public void GivenAnInvalidAvailableOn_Validate_ShouldHaveAValidationError()
        {
            _validator.ShouldHaveValidationErrorFor(listing => listing.AvailableOn, DateTime.MinValue);
        }
    }
}
