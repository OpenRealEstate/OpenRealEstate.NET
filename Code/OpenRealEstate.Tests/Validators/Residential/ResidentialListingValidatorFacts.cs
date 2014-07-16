using System;
using System.IO;
using System.Linq;
using FluentValidation;
using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Services.RealEstateComAu;
using OpenRealEstate.Validation.Residential;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Validators.Residential
{
    public class ResidentialListingValidatorFacts
    {
        public class RuleSetFacts
        {
            private static ResidentialListing CreateListing()
            {
                var xml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
                var transmogrifier = new ReaXmlTransmorgrifier();
                return transmogrifier.ConvertTo(xml).Listings.First().Listing as ResidentialListing;
            }

            [Fact]
            public void GivenACommonRuleSet_Validate_ShouldNotHaveAnyValidationErrors()
            {
                // Arrange.
                var listing = CreateListing();
                var validator = new ResidentialListingValidator();

                // Act.
                var result = validator.Validate(listing,
                    ruleSet: ResidentialListingValidator.CurrentRuleSet);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenAnIncompleteListingAndACommonRuleSet_Validate_ShouldHaveSomeValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();

                // Act.
                var result = validator.Validate(new ResidentialListing(),
                    ruleSet: ResidentialListingValidator.CurrentRuleSet);

                // Assert.
                result.ShouldNotBe(null);
                result.Errors.Count.ShouldBe(6);
            }
        }

        public class SimpleValidationFacts
        {
            private readonly ResidentialListingValidator _validator;

            public SimpleValidationFacts()
            {
                _validator = new ResidentialListingValidator();
            }

            [Fact]
            public void GivenAPropertyType_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.PropertyType, PropertyType.Townhouse);
            }

            [Fact]
            public void GivenAnUnknownPropertyType_Validate_ShouldHaveAValidationError()
            {
                _validator.ShouldHaveValidationErrorFor(listing => listing.PropertyType, PropertyType.Unknown);
            }

            [Fact]
            public void GivenAnAuctionOn_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.AuctionOn, DateTime.UtcNow);
            }

            [Fact]
            public void GivenAnInvalidAuctionOn_Validate_ShouldHaveAValidationError()
            {
                _validator.ShouldHaveValidationErrorFor(listing => listing.AuctionOn, DateTime.MinValue);
            }

            [Fact]
            public void GivenANullAuctionOn_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.AuctionOn, (DateTime?) null);
            }
        }
    }
}