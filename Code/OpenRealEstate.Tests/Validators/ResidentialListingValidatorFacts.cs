using FluentValidation.TestHelper;
using OpenRealEstate.Validation.Residential;
using Xunit;

namespace OpenRealEstate.Tests.Validators
{
    public class ResidentialListingValidatorFacts
    {
        private readonly ResidentialListingValidator _listingValidator;

        public ResidentialListingValidatorFacts()
        {
            _listingValidator = new ResidentialListingValidator();
        }

        [Fact]
        public void GivenAnAgentId_Validate_ShouldNotHaveValidationErrors()
        {
            _listingValidator.ShouldNotHaveValidationErrorFor(listing => listing.AgencyId, "a");
        }
    }
}