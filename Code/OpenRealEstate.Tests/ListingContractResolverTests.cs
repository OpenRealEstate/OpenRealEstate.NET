using Newtonsoft.Json;
using OpenRealEstate.Services.Json;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class ListingContractResolverTests
    {
        public class SerializeObjectTests
        {
            [Fact]
            public void GivenAnListing_SerializeObject_ReturnsSomeJsonWithSomePropertiesThatWereIgnored()
            {
                // Arrange.
                var listing = TestHelperUtilities.ResidentialListing();
                var jsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ListingContractResolver()
                };

                // Act.
                var json = JsonConvert.SerializeObject(listing, jsonSettings);

                // Assert.
                json.ShouldNotContain("modified");
            }
        }
    }
}