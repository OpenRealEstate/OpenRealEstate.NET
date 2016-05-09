using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FluentValidation;
using FluentValidation.TestHelper;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Residential;
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
                return transmogrifier.Parse(xml).Listings.First().Listing as ResidentialListing;
            }

            [Theory]
            [InlineData("default")]
            [InlineData(ResidentialListingValidator.NormalRuleSet)]
            [InlineData(ResidentialListingValidator.StrictRuleSet)]
            public void GivenTheFileREAResidentialCurrentXml_Validate_ShouldNotHaveValidationErrors(string ruleSet)
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing();

                // Act.
                var result = validator.Validate(listing, ruleSet: ruleSet);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Theory]
            [InlineData("default", 5)] // Aggregate root x2 + Listing x3.
            [InlineData(ResidentialListingValidator.NormalRuleSet, 12)] // default + 9
            [InlineData(ResidentialListingValidator.StrictRuleSet, 13)] // Minimum + 1
            public void GivenAnIncompleteListingAndARuleSet_Validate_ShouldHaveValidationErrors(string ruleSet, int numberOfErrors)
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing();
                listing.Id = null;
                listing.UpdatedOn = DateTime.MinValue;
                listing.AgencyId = null;
                listing.StatusType = StatusType.Unknown;
                listing.CreatedOn = DateTime.MinValue;
                listing.Agents.First().Name = null; // Not legit.
                listing.Images.First().Url = null; // Not legit.
                listing.FloorPlans.First().Url = null; // Not legit.
                listing.Videos.First().Url = null; // Not legit.
                listing.Inspections.First().OpensOn = DateTime.MinValue; // Not legit.
                listing.LandDetails.Area.Value = -1; // Not legit.

                // The sum of the 3 carparking properties can't exceed 255 (a byte).
                listing.Features.CarParking.Carports = 200;
                listing.Features.CarParking.Garages = 200;

                listing.Links = new List<string>
                {
                    "http://aa.bb.cc/dd/ee?ff=ggg", // Legit.
                    "sdfdsfsdfdf" // Not legit.
                };

                // Act.
                var result = validator.Validate(listing, ruleSet: ruleSet);

                // Assert.
                result.ShouldNotBeNull();
                result.Errors.Count.ShouldBe(numberOfErrors);
            }

            [Fact]
            public void GivenAListingWithAMissingSuburbAddressAndAMinimumRuleSet_Validate_ShouldHaveValidationErrors()
            {
                // Arrange.
                var validator = new ResidentialListingValidator();
                var listing = CreateListing();
                listing.Address.Suburb = null;

                // Act.
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.NormalRuleSet);

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
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.NormalRuleSet);

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
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.NormalRuleSet);

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
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.NormalRuleSet);

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
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.NormalRuleSet);

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
                var result = validator.Validate(listing, ruleSet: ResidentialListingValidator.NormalRuleSet);

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
                return transmogrifier.Parse(xml).Listings.First().Listing as ResidentialListing;
            }

            [Fact]
            public void GivenAPropertyType_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.PropertyType, 
                    PropertyType.Townhouse, 
                    ResidentialListingValidator.NormalRuleSet);
            }

            [Fact]
            public void GivenAnUnknownPropertyType_Validate_ShouldHaveAValidationError()
            {
                _validator.ShouldHaveValidationErrorFor(x => x.PropertyType, 
                    PropertyType.Unknown, 
                    ResidentialListingValidator.NormalRuleSet);
            }

            [Fact]
            public void GivenAnAuctionOn_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.AuctionOn, 
                    DateTime.UtcNow,
                    ResidentialListingValidator.NormalRuleSet);
            }

            [Fact]
            public void GivenAnInvalidAuctionOn_Validate_ShouldHaveAValidationError()
            {
                _validator.ShouldHaveValidationErrorFor(listing => listing.AuctionOn, 
                    DateTime.MinValue,
                    ResidentialListingValidator.NormalRuleSet);
            }

            [Fact]
            public void GivenANullAuctionOn_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.AuctionOn, 
                    (DateTime?) null,
                    ResidentialListingValidator.NormalRuleSet);
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
                //    ResidentialListingValidator.NormalRuleSet);
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
                    ResidentialListingValidator.NormalRuleSet);
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
                //    ResidentialListingValidator.NormalRuleSet);

                var listing = new ResidentialListing
                {
                    Links = links
                };

                var result = _validator.Validate(listing,
                    ruleSet: ResidentialListingValidator.NormalRuleSet);

                // Assert.
                result.Errors.ShouldNotContain(x =>
                    x.ErrorMessage == $"Link '{uri}' must be a valid URI. eg: http://www.SomeWebSite.com.au");
            }

            [Theory]
            [InlineData("Httpasd://www.SomeDomain.com")]
            [InlineData("aasdasd")]
            [InlineData("")]
            [InlineData("ftp://www.a.b.c.com")]
            [InlineData("Htttd://www.SomeDomain.com")] // 3x t's in http.
            [InlineData("www.SomeDomain.com")] // No scheme.
            [InlineData("!2134242")]
            public void GivenAnInvalidUri_Validate_ShouldHaveAValidationError(string uri)
            {
                // Arrange.
                var links = new ReadOnlyCollection<string>(new[] { uri });

                // Act & Assert.
                
                // NOTE: ShouldHaveValidationErrorFor has a bug in it: https://github.com/JeremySkinner/FluentValidation/issues/238

                //_validator.ShouldHaveValidationErrorFor(l => l.Links, 
                //    links, 
                //    ResidentialListingValidator.NormalRuleSet);

                var listing = new ResidentialListing
                {
                    Links = links
                };

                var result = _validator.Validate(listing, ruleSet: ResidentialListingValidator.StrictRuleSet);

                // Assert.
                result.Errors.ShouldContain(x => x.ErrorMessage == $"Link '{uri}' must be a valid URI. eg: http://www.SomeWebSite.com.au");
            }
        }
    }
}