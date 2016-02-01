using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Residential;
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
            [Fact]
            public void GivenSomeResidentialJson_ConvertTo_ReturnsAListing()
            {
                // Arrange.
                var json = File.ReadAllText($"Sample Data\\Transmorgrifiers\\Json\\Residential\\Residential-Current.json");
                var transmorgrifier = new JsonTransmorgrifier();

                // Act.
                var result = transmorgrifier.ConvertTo(json);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.Listings.First().SourceData.ShouldNotBeNull();
                var listing = result.Listings.First().Listing;
                TestHelperUtilities.AssertResidentialListing(listing as ResidentialListing, TestHelperUtilities.ResidentialListing());
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
