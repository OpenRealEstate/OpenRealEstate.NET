using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;
using OpenRealEstate.Services.Json;
using Shouldly;
using Xunit;
using Xunit.Extensions;

namespace OpenRealEstate.Tests
{
    public class JsonTransmorgrifierTests
    {
        public class ConvertToTests
        {
            [Theory]
            [InlineData("Sample Data\\Transmorgrifiers\\Json\\Residential\\Residential-Current.json", typeof(ResidentialListing))]
            [InlineData("Sample Data\\Transmorgrifiers\\Json\\Rental\\Rental-Current.json", typeof(RentalListing))]
            [InlineData("Sample Data\\Transmorgrifiers\\Json\\Land\\Land-Current.json", typeof(LandListing))]
            [InlineData("Sample Data\\Transmorgrifiers\\Json\\Rural\\Rural-Current.json", typeof(RuralListing))]
            public void GivenSomeResidentialJson_ConvertTo_ReturnsAListing(string jsonPath, Type listingType)
            {
                // Arrange.
                var json = File.ReadAllText(jsonPath);
                var transmorgrifier = new JsonTransmorgrifier();

                // Act.
                var result = transmorgrifier.ConvertTo(json);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.Listings.First().SourceData.ShouldNotBeNull();
                var listing = result.Listings.First().Listing;
                if (listingType == typeof(ResidentialListing))
                {
                    TestHelperUtilities.AssertResidentialListing(listing as ResidentialListing,
                        TestHelperUtilities.ResidentialListing());
                }
                else if (listingType == typeof(RentalListing))
                {
                    TestHelperUtilities.AssertRentalListing(listing as RentalListing, 
                        TestHelperUtilities.RentalListing());
                }
                else if (listingType == typeof(LandListing))
                {
                    TestHelperUtilities.AssertLandListing(listing as LandListing,
                        TestHelperUtilities.LandListing());
                }
                else if (listingType == typeof(RuralListing))
                {
                    TestHelperUtilities.AssertRuralListing(listing as RuralListing,
                        TestHelperUtilities.RuralListing());
                }
                else
                {
                    throw new Exception($"Failed to assert the suggested type: '{listingType}'.");
                }
            }

            [Fact]
            public void GivenSomeIllegalJson_ConvertTo_ReturnsAndError()
            {
                // Arrange.
                const string json = "sadsdf";
                var transmorgrifier = new JsonTransmorgrifier();

                // Act.
                var result = transmorgrifier.ConvertTo(json);

                // Assert.
                result.Listings.ShouldBeNull();
                result.UnhandledData.ShouldBeNull();
                result.Errors.Count.ShouldBe(1);
                result.Errors.First().ExceptionMessage.ShouldBe("Unexpected character encountered while parsing value: s. Path '', line 0, position 0.");
                result.Errors.First().InvalidData.ShouldNotBeNullOrWhiteSpace();
            }

            [Fact]
            public void GivenSomeJsonWithAnMissingListingType_ConvertTo_ReturnsAnError()
            {
                // Arrange.
                var json = File.ReadAllText($"Sample Data\\Transmorgrifiers\\Json\\Residential\\Residential-Current.json")
                    .Replace("\"Residential\",", "\"blah\",");
                
                var transmorgrifier = new JsonTransmorgrifier();

                // Act.
                var result = transmorgrifier.ConvertTo(json);

                // Assert.
                result.Listings.ShouldBeNull();
                result.UnhandledData.ShouldBeNull();
                result.Errors.Count.ShouldBe(1);
                result.Errors.First().ExceptionMessage.ShouldBe("Invalid value found in the expected field 'ListingType'. Only the following values (ie. listing types) as supported: residential, rental, land or rural.");
                result.Errors.First().InvalidData.ShouldNotBeNullOrWhiteSpace();
            }
        }
    }
}
