using System;
using System.Linq;
using OpenRealEstate.Core.Land;
using OpenRealEstate.Core.Rental;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Core.Rural;
using OpenRealEstate.Services.Json;
using OpenRealEstate.Tests.Transmorgrifiers.REA;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class JsonTransmorgrifierTests
    {
        public class ParseTests : TestHelperUtilities
        {
            [Theory]
            [InlineData(typeof(ResidentialListing), 1)]
            [InlineData(typeof(ResidentialListing), 3)]
            [InlineData(typeof(RentalListing), 1)]
            [InlineData(typeof(LandListing), 1)]
            [InlineData(typeof(RuralListing), 1)]
            public void GivenSomeValidJson_Parse_ReturnsAListing(Type listingType, int listingCount)
            {
                // Arrange.
                var existingListing = CreateListings(listingType, listingCount);
                var json = JsonConvertHelpers.SerializeObject(existingListing);
                var transmorgrifier = new JsonTransmorgrifier();

                // Act.
                var result = transmorgrifier.Parse(json);

                // Assert.
                result.Listings.Count.ShouldBe(listingCount);
                result.UnhandledData.Count.ShouldBe(0);
                result.Errors.Count.ShouldBe(0);

                for (var i = 0; i < result.Listings.Count; i++)
                {
                    if (listingType == typeof(ResidentialListing))
                    {
                        ResidentialListingAssertHelpers.AssertResidentialListing(
                            (ResidentialListing) result.Listings[i].Listing,
                            (ResidentialListing) existingListing[i]);
                    }
                    else if (listingType == typeof(RentalListing))
                    {
                        RentalListingAssertHelpers.AssertRuralListing(
                            (RentalListing) result.Listings[i].Listing,
                            (RentalListing) existingListing[i]);
                    }
                    else if (listingType == typeof(LandListing))
                    {
                        LandListingAssertHelpers.AssertLandListing(
                            (LandListing) result.Listings[i].Listing,
                            (LandListing) existingListing[i]);
                    }
                    else if (listingType == typeof(RuralListing))
                    {
                        RuralListingAssertHelpers.AssertRuralListing(
                            (RuralListing) result.Listings[i].Listing,
                            (RuralListing) existingListing[i]);
                    }
                    else
                    {
                        throw new Exception($"Failed to assert the suggested type: '{listingType}'.");
                    }
                }
            }

            [Fact]
            public void GivenSomeIllegalJson_Parse_ReturnsAndError()
            {
                // Arrange.
                const string json = "sadsdf";
                var transmorgrifier = new JsonTransmorgrifier();

                // Act.
                var result = transmorgrifier.Parse(json);

                // Assert.
                result.Listings.Count.ShouldBe(0);
                result.UnhandledData.Count.ShouldBe(0);
                result.Errors.Count.ShouldBe(1);
                result.Errors.First()
                    .ExceptionMessage.ShouldBe(
                        "Unexpected character encountered while parsing value: s. Path '', line 0, position 0.");
                result.Errors.First().InvalidData.ShouldNotBeNullOrWhiteSpace();
            }

            [Fact]
            public void GivenSomeJsonWithAnMissingListingType_Parse_ReturnsAnError()
            {
                // Arrange.
                var existingListing = CreateListings(typeof(ResidentialListing), 1);
                var json = JsonConvertHelpers.SerializeObject(existingListing).Replace("\"Residential\",", "\"blah\",");

                var transmorgrifier = new JsonTransmorgrifier();

                // Act.
                var result = transmorgrifier.Parse(json);

                // Assert.
                result.Listings.Count.ShouldBe(0);
                result.UnhandledData.Count.ShouldBe(0);
                result.Errors.Count.ShouldBe(1);
                result.Errors.First()
                    .ExceptionMessage.ShouldBe(
                        "Invalid value found in the expected field 'ListingType'. Only the following values (ie. listing types) as supported: residential, rental, land or rural.");
                result.Errors.First().InvalidData.ShouldNotBeNullOrWhiteSpace();
            }
        }
    }
}