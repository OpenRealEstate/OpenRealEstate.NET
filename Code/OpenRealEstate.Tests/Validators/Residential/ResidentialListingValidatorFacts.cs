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
            private static ResidentialListing CreateListing(string fileName = null)
            {
                var xml = File.ReadAllText(fileName
                                           ?? "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
                var transmogrifier = new ReaXmlTransmorgrifier();
                return transmogrifier.ConvertTo(xml).Listings.First().Listing as ResidentialListing;
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentXml_Validate_ShouldNotHaveValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing();

                // Act.
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenAnIncompleteListingAndAMinimumRuleSet_Validate_ShouldHaveValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();

                // Act.
                var result = validator.Validate(new ResidentialListing(),
                    ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.ShouldNotBe(null);
                result.Errors.Count.ShouldBe(9);
            }

            [Fact]
            public void GivenAListingWithAMissingSuburbAddressAndAMinimumRuleSet_Validate_ShouldHaveValidationErrors()
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
            public void GivenAListingWithAStreetNumberButMissingStreetAndAMinimumRuleSet_Validate_ShouldHaveValidationErrors()
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
            public void GivenAListingWithAMissingAgentNameAndAMinimumRuleSet_Validate_ShouldHaveValidationErrors()
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
            public void GivenTheFileREAResidentialSoldXml_Validate_ShouldNotHaveValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold.xml");

                // Act.
                var result = validator.Validate(listing);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenTheFileREAResidentialSoldXmlAndTheMinimumRuleSet_Validate_ShouldHaveValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold.xml");

                // Act.
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.Count.ShouldBe(4);
            }

            [Fact]
            public void GivenTheFileREAResidentialWithdrawnXml_Validate_ShouldNotHaveValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Withdrawn.xml");

                // Act.
                var result = validator.Validate(listing);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenTheFileREAResidentialWithdrawnXmlAndTheMinimumRuleSet_Validate_ShouldHaveValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Withdrawn.xml");

                // Act.
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.Count.ShouldBe(4);
            }

            [Fact]
            public void GivenTheFileREAResidentialOffMarketXml_Validate_ShouldNotHaveValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-OffMarket.xml");

                // Act.
                var result = validator.Validate(listing);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenTheFileREAResidentialOffMarketXmlAndTheMinimumRuleSet_Validate_ShouldHaveValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-OffMarket.xml");

                // Act.
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.Count.ShouldBe(4);
            }
        }

        public class SimpleValidationFacts
        {
            private readonly ResidentialListingValidator _validator;

            public SimpleValidationFacts()
            {
                _validator = new ResidentialListingValidator();
            }

            private static ResidentialListing CreateListing(string fileName = null)
            {
                var xml = File.ReadAllText(fileName
                                           ?? "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
                var transmogrifier = new ReaXmlTransmorgrifier();
                return transmogrifier.ConvertTo(xml).Listings.First().Listing as ResidentialListing;
            }

            [Fact(Skip = "Need to figure out how to use the RuleSet in this extension method.")]
            public void GivenAPropertyType_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.PropertyType, PropertyType.Townhouse);
            }

            [Fact]
            public void GivenAnUnknownPropertyType_Validate_ShouldHaveAValidationError()
            {
                // Arrange.
                var listing = CreateListing();
                listing.PropertyType = PropertyType.Unknown;

                // Act.
                var result = _validator.Validate(listing, ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.ShouldContain(x => x.PropertyName == "PropertyType");
            }

            [Fact(Skip = "Need to figure out how to use the RuleSet in this extension method.")]
            public void GivenAnAuctionOn_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.AuctionOn, DateTime.UtcNow);
            }

            [Fact]
            public void GivenAnInvalidAuctionOn_Validate_ShouldHaveAValidationError()
            {
                // Arrange.
                var listing = CreateListing();
                listing.AuctionOn = DateTime.MinValue;

                // Act.
                var result = _validator.Validate(listing, ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.ShouldContain(x => x.PropertyName == "AuctionOn");
            }

            [Fact(Skip = "Need to figure out how to use the RuleSet in this extension method.")]
            public void GivenANullAuctionOn_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.AuctionOn, (DateTime?) null);
            }
        }
    }
}