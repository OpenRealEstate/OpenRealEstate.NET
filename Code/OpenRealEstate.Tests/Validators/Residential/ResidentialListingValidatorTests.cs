using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FluentValidation;
using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Services.RealEstateComAu;
using OpenRealEstate.Validation;
using OpenRealEstate.Validation.Residential;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Validators.Residential
{
    public class ResidentialListingValidatorTests
    {
        public class RuleSetTests
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
                result.ShouldNotBeNull();
                result.Errors.Count.ShouldBe(8);
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
                result.Errors.Count.ShouldBe(3);
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
                result.Errors.Count.ShouldBe(3);
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
                result.Errors.Count.ShouldBe(3);
            }
        }

        public class SimpleValidationTests
        {
            private readonly ResidentialListingValidator _validator;

            public SimpleValidationTests()
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

            [Fact]
            public void GivenAPropertyType_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.PropertyType, 
                    PropertyType.Townhouse, 
                    ResidentialListingValidator.MinimumRuleSet);
            }

            [Fact]
            public void GivenAnUnknownPropertyType_Validate_ShouldHaveAValidationError()
            {
                _validator.ShouldHaveValidationErrorFor(x => x.PropertyType, 
                    PropertyType.Unknown, 
                    ResidentialListingValidator.MinimumRuleSet);
            }

            [Fact]
            public void GivenAnAuctionOn_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.AuctionOn, 
                    DateTime.UtcNow,
                    ResidentialListingValidator.MinimumRuleSet);
            }

            [Fact]
            public void GivenAnInvalidAuctionOn_Validate_ShouldHaveAValidationError()
            {
                _validator.ShouldHaveValidationErrorFor(listing => listing.AuctionOn, 
                    DateTime.MinValue,
                    ResidentialListingValidator.MinimumRuleSet);
            }

            [Fact]
            public void GivenANullAuctionOn_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.AuctionOn, 
                    (DateTime?) null,
                    ResidentialListingValidator.MinimumRuleSet);
            }

            [Fact(Skip = "Shouldly doesn't like ReadOnlyCollections")]
            public void GivenAFewLinksThatAreUrisAndTheMinimumRuleSet_Validate_ShouldNotHaveValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var links = new List<string>
                {
                    "http://www.google.com",
                    "https://www.microsoft.com",
                    "https://www.github.com"
                };

                // Act & Assert.
                // This fails because shouldly doesn't like ReadOnly collections.
                //validator.ShouldNotHaveValidationErrorFor(listing => listing.Links,
                //    links.AsReadOnly(),
                //    ResidentialListingValidator.MinimumRuleSet);
            }

            [Fact(Skip = "Shouldly doesn't like ReadOnlyCollections")]
            public void GivenAFewLinksThatAreUrisButOneIsInvalidAndTheMinimumRuleSet_Validate_ShouldNotHaveValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var links = new List<string>
                {
                    "http://www.google.com",
                    "https://www.microsoft.com",
                    "https://www.github.com",
                    "aaaaa"
                };

                // Act & Assert.
                // This fails because shouldly doesn't like ReadOnly collections.
                validator.ShouldHaveValidationErrorFor(listing => listing.Links.ToList(),
                    links,
                    ResidentialListingValidator.MinimumRuleSet);
            }

            [Theory]
            [InlineData("Http://www.SomeDomain.com")]
            [InlineData("https://www.SomeDomain.com")]
            [InlineData("http://www.SomeDomain.com.au")]
            [InlineData(null)]
            public void GivenAValidUri_Validate_ShouldNotHaveAValidationError(string uri)
            {
                // Arrange.
                // NOTE: We don't want to check for an empty string here, because that
                //       is contained in the 'bad' checks below.
                var links = uri == null
                    ? null
                    : new ReadOnlyCollection<string>(new[] {uri});

                // Act & Assert.

                // NOTE: ShouldNotHaveValidationErrorFor has a bug in it: https://github.com/JeremySkinner/FluentValidation/issues/238

                //_validator.ShouldNotHaveValidationErrorFor(x => x.Links, 
                //    links,
                //    ResidentialListingValidator.MinimumRuleSet);

                var listing = new ResidentialListing
                {
                    Links = links
                };

                var result = _validator.Validate(listing,
                    ruleSet: ResidentialListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.ShouldNotContain(x =>
                    x.ErrorMessage == $"Link '{uri}' must be a valid URI. eg: http://www.SomeWebSite.com.au");
            }
        }
    }
}