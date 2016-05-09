using System;
using System.IO;
using System.Linq;
using FluentValidation;
using FluentValidation.TestHelper;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Rural;
using OpenRealEstate.Services.RealEstateComAu;
using OpenRealEstate.Validation;
using OpenRealEstate.Validation.Rural;
using Shouldly;
using Xunit;
using RuralCategoryType = OpenRealEstate.Core.Rural.CategoryType;

namespace OpenRealEstate.Tests.Validators.Rural
{
    public class RuralListingValidatorFacts
    {
        public class RuleSetFacts
        {
            private static RuralListing CreateListing(string fileName = null)
            {
                var xml = File.ReadAllText(fileName 
                    ?? "Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Current.xml");
                var transmogrifier = new ReaXmlTransmorgrifier();
                return transmogrifier.Parse(xml).Listings.First().Listing as RuralListing;
            }

            [Fact]
            public void GivenAMinimumRuleSet_Validate_ShouldNotHaveAnyValidationErrors()
            {
                // Arrange.
                var listing = CreateListing(); 
                var validator = new RuralListingValidator();
                
                // Act.
                var result = validator.Validate(listing,
                    ruleSet: RuralListingValidator.NormalRuleSet);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenAnAuctionDataAndACommonRuleSet_Validate_ShouldNotHaveAnyValidationErrors()
            {
                // Arrange.
                var listing = CreateListing();
                var validator = new RuralListingValidator();
                listing.AuctionOn = DateTime.UtcNow;

                // Act.
                var result = validator.Validate(listing,
                    ruleSet: RuralListingValidator.NormalRuleSet);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenAnInvalidAuctionDataAndACommonRuleSet_Validate_ShouldNotHaveAnyValidationErrors()
            {
                // Arrange.
                var listing = CreateListing();
                var validator = new RuralListingValidator();
                listing.AuctionOn = DateTime.UtcNow;

                // Act.
                var result = validator.Validate(listing,
                    ruleSet: RuralListingValidator.NormalRuleSet);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            //[Fact]
            //public void GivenAnInvalidAuctionDataAndACommonRuleSet_Validate_ShouldNotHaveAnyValidationErrors()
            //{
            //    // Arrange.
            //    var listing = CreateListing();
            //    var validator = new RuralListingValidator();
            //    listing.AuctionOn = DateTime.UtcNow;

            //    // Act.
            //    var result = validator.Validate(listing,
            //        ruleSet: RuralListingValidator.NormalRuleSet);

            //    // Assert.
            //    result.Errors.Count.ShouldBe(0);
            //}

            [Fact]
            public void GivenAListingAndADefaultRuleSet_Validate_ShouldHaveNotHaveAnyValidationErrors()
            {
                // Arrange.
                var listing = CreateListing("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Withdrawn.xml");
                var validator = new RuralListingValidator();

                // Act.
                var result = validator.Validate(listing);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }
        }

        public class SimpleValidationFacts
        {
            private readonly RuralListingValidator _validator;

            public SimpleValidationFacts()
            {
                _validator = new RuralListingValidator();
            }

            [Fact]
            public void GivenACategoryType_Validate_ShouldNotHaveAValidationError()
            {
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.CategoryType, 
                    RuralCategoryType.Cropping,
                    RuralListingValidator.NormalRuleSet);
            }

            [Fact]
            public void GivenAnUnknownCategoryType_Validate_ShouldHaveAValidationError()
            {
                _validator.ShouldHaveValidationErrorFor(listing => listing.CategoryType,
                    RuralCategoryType.Unknown,
                    RuralListingValidator.NormalRuleSet);
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

            [Fact]
            public void GivenASalePricing_Validate_ShouldNotHaveAValidationError()
            {
                // Arrange.
                var salePricing = new SalePricing
                {
                    SalePrice = 1234,
                    SalePriceText = "Contact agent"
                };

                // Act & Assert.
                _validator.ShouldHaveChildValidator(listing => listing.Pricing, typeof (SalePricingValidator));
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.Pricing, salePricing);
            }
        }
    }
}
