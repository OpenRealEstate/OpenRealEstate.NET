using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.FakeData;
using OpenRealEstate.Services.Json;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class JsonConvertHelpersTests
    {
        public class SerializeObjectTests
        {
            [Fact]
            public void GivenAListing_Serialize_ReturnsSomeJson()
            {
                // Arrange.
                var listings = FakeListings.CreateAFakeListing<ResidentialListing>();

                // Act.
                var json = JsonConvertHelpers.SerializeObject(listings);

                // Assert.
                json.ShouldStartWith("{");
                json.ShouldEndWith("}");
            }

            [Fact]
            public void GivenSomeListings_Serialize_ReturnsSomeJson()
            {
                // Arrange.
                var listings = FakeListings.CreateFakeListings<ResidentialListing>();

                // Act.
                var json = JsonConvertHelpers.SerializeObject(listings);

                // Assert.
                json.ShouldStartWith("[");
                json.ShouldEndWith("]");
            }
        }
    }
}