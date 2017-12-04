using System;
using System.IO;
using System.Linq;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Land;
using OpenRealEstate.Core.Rental;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Core.Rural;
using OpenRealEstate.Services.RealEstateComAu;
using OpenRealEstate.Validation;
using Shouldly;
using Xunit;
using LandCategoryType = OpenRealEstate.Core.Land.CategoryType;

namespace OpenRealEstate.Tests.Validators
{
    public class ValidatorMediatorTests
    {
        public class ValidateTests
        {
            private static Listing GetListing(Type listingType)
            {
                string fileName = null;
                if (listingType == typeof(ResidentialListing))
                {
                    fileName = "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml";
                }
                else if (listingType == typeof(RentalListing))
                {
                    fileName = "Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Current.xml";
                }
                else if (listingType == typeof(RuralListing))
                {
                    fileName = "Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Current.xml";
                }
                else if (listingType == typeof(LandListing))
                {
                    fileName = "Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current.xml";
                }

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    throw new Exception("No valid type provided. Must be a 'Listing' type.");
                }

                return GetListing(fileName);
            }

            private static Listing GetListing(string fileName)
            {
                fileName.ShouldNotBeNullOrEmpty();

                var reaXml = File.ReadAllText(fileName);
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                return reaXmlTransmorgrifier.Parse(reaXml).Listings.First().Listing;
            }

            [Fact]
            public void GivenACurrentResidentialListing_Validate_ValidatesTheListingWithNoErrors()
            {
                // Arrange.
                var listing = GetListing(typeof(ResidentialListing));

                // Arrange.
                var result = ValidatorMediator.Validate(listing);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenACurrentResidentialListingWithSomeRequiredMissingData_Validate_ValidatesTheListingWithSomeErrors()
            {
                // Arrange.
                var listing = (ResidentialListing) GetListing(typeof(ResidentialListing));
                listing.Id = null;
                listing.AgencyId = null;
                listing.Pricing.SalePrice = -1;

                // Arrange.
                var result = ValidatorMediator.Validate(listing);

                // Assert.
                result.Errors.Count.ShouldBe(3);
            }

            [Fact]
            public void GivenALandListing_Validate_ValidatesTheListingWithNoErrors()
            {
                // Arrange.
                var listing = GetListing(typeof(LandListing));

                // Arrange.
                var result = ValidatorMediator.Validate(listing);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenALandListingWithSomeRequiredMissingData_Validate_ValidatesTheListingWithSomeErrors()
            {
                // Arrange.
                var listing = (LandListing) GetListing(typeof(LandListing));
                listing.Id = null;
                listing.AgencyId = null;
                listing.CategoryType = LandCategoryType.Unknown; // That's allowed, now :(

                // Arrange.
                var result = ValidatorMediator.Validate(listing, ListingValidatorRuleSet.Minimum);

                // Assert.
                result.Errors.Count.ShouldBe(2);
            }

            [Fact]
            public void GivenARentalListing_Validate_ValidatesTheListingWithNoErrors()
            {
                // Arrange.
                var listing = GetListing(typeof(RentalListing));

                // Arrange.
                var result = ValidatorMediator.Validate(listing);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenARentalListingWithSomeRequiredMissingData_Validate_ValidatesTheListingWithSomeErrors()
            {
                // Arrange.
                var listing = (RentalListing) GetListing(typeof(RentalListing));
                listing.Id = null;
                listing.AgencyId = null;
                listing.PropertyType = PropertyType.Unknown;

                // Arrange.
                var result = ValidatorMediator.Validate(listing);

                // Assert.
                result.Errors.Count.ShouldBe(3);
            }

            [Fact]
            public void GivenARuralListingWithSomeRequiredMissingData_Validate_ValidatesTheListingWithSomeErrors()
            {
                // Arrange.
                var listing = (RuralListing) GetListing(typeof(RuralListing));
                listing.Id = null;
                listing.AgencyId = null;
                listing.Pricing.SalePrice = -1;

                // Arrange.
                var result = ValidatorMediator.Validate(listing);

                // Assert.
                result.Errors.Count.ShouldBe(3);
            }

            [Fact]
            public void GivenARurallListing_Validate_ValidatesTheListingWithNoErrors()
            {
                // Arrange.
                var listing = GetListing(typeof(RuralListing));

                // Arrange.
                var result = ValidatorMediator.Validate(listing);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenAWithdrawnResidentialListing_Validate_ValidatesTheListingWithNoErrors()
            {
                // Arrange.
                var listing = GetListing("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Withdrawn.xml");

                // Arrange.
                var result = ValidatorMediator.Validate(listing, ListingValidatorRuleSet.Minimum);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenAWithdrawnResidentialListingWithStrictValidation_Validate_ValidatesTheListingWithNoErrors()
            {
                // Arrange.
                var listing = GetListing("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Withdrawn.xml");

                // Arrange.
                var result = ValidatorMediator.Validate(listing, ListingValidatorRuleSet.Strict);

                // Assert.
                result.Errors.Count.ShouldBe(3);
            }
        }
    }
}