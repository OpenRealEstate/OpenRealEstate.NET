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
                    ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenAnIncompleteListingAndAMinimumRuleSet_Validate_ShouldHaveSomeValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();

                // Act.
                var result = validator.Validate(new ResidentialListing(),
                    ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.ShouldNotBe(null);
                result.Errors.Count.ShouldBe(8);
            }

            [Fact]
            public void GivenAListingWithAMissingSuburbAddress_Validate_ShouldHaveSomeValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing();
                listing.Address.Suburb = null;

                // Act.
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.ShouldContain(x => x.PropertyName == "Address.Suburb");
            }

            [Fact]
            public void GivenAListingWithAStreetNumberButMissingStreet_Validate_ShouldHaveSomeValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing();
                listing.Address.Street = string.Empty;

                // Act.
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.ShouldContain(x => x.PropertyName == "Address.Street");
            }

            [Fact]
            public void GivenAListingWithAMissingAgentName_Validate_ShouldHaveSomeValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing();
                listing.Agents.First().Name = string.Empty;

                // Act.
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.ShouldContain(x => x.PropertyName == "Agents[0].Name");
            }

            [Fact]
            public void GivenAListingWithAMissingImageUrl_Validate_ShouldHaveSomeValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing();
                listing.Images.First().Url = string.Empty;

                // Act.
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.ShouldContain(x => x.PropertyName == "Images[0].Url");
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