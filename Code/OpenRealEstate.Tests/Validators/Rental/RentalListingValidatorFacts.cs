using System;
using System.IO;
using System.Linq;
using FluentValidation;
using FluentValidation.TestHelper;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Services.RealEstateComAu;
using OpenRealEstate.Validation.Rental;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Validators.Rental
{
    public class RentalListingValidatorFacts
    {
        public class SimpleValidationFacts
        {
            private readonly RentalListingValidator _validator;

            public SimpleValidationFacts()
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
                _validator.ShouldNotHaveValidationErrorFor(listing => listing.AvailableOn, (DateTime?) null);
            }

            [Fact]
            public void GivenAnInvalidAvailableOn_Validate_ShouldHaveAValidationError()
            {
                _validator.ShouldHaveValidationErrorFor(listing => listing.AvailableOn, DateTime.MinValue);
            }
        }

        public class ValidationFacts
        {
            private static RentalListing CreateListing(string fileName = null)
            {
                var xml = File.ReadAllText(fileName
                                           ?? "Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Current.xml");
                var transmogrifier = new ReaXmlTransmorgrifier();
                return transmogrifier.ConvertTo(xml).Listings.First().Listing as RentalListing;
            }
            
            [Fact]
            public void GivenARentalListing_Validate_ShouldNotHaveAnyValidationErrors()
            {
                // Arrange.
                var validator = new RentalListingValidator();
                var listing = CreateListing();

                // Act.
                var result = validator.Validate(listing, ruleSet: RentalListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenAnIncompleteRentalListing_Validate_ShouldHaveAnyValidationErrors()
            {
                // Arrange.
                var validator = new RentalListingValidator();
                
                // Act.
                var result = validator.Validate(new RentalListing(),
                    ruleSet: RentalListingValidator.MinimumRuleSet);

                // Assert.
                result.Errors.Count.ShouldBe(9);
                result.Errors.ShouldContain(x => x.PropertyName == "AgencyId");
                result.Errors.ShouldContain(x => x.PropertyName == "StatusType");
                result.Errors.ShouldContain(x => x.PropertyName == "CreatedOn");
                result.Errors.ShouldContain(x => x.PropertyName == "Id");
                result.Errors.ShouldContain(x => x.PropertyName == "UpdatedOn");
                result.Errors.ShouldContain(x => x.PropertyName == "Title");
                result.Errors.ShouldContain(x => x.PropertyName == "Description");
                result.Errors.ShouldContain(x => x.PropertyName == "Address");
                result.Errors.ShouldContain(x => x.PropertyName == "PropertyType");
            }
        }
    }
}