using System.IO;
using Newtonsoft.Json;
using OpenRealEstate.Core;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class FranchiseTests
    {
        public class DeserializationTests
        {
            [Theory]
            [InlineData("Sample Data\\Franchises\\Active-Sample.Franchise.1.json", "sample.franchise.1")]
            [InlineData("Sample Data\\Franchises\\Active-Sample.Franchise.1-BadData.json", null)]
            public void GivenAFranchiseJsonFileActiveSampleFranchise1_Deserialization_ValidatesAFranchise(string path,
                                                                                                          string id)
            {
                // Arrange.
                var json = File.ReadAllText(path);

                // Act.
                var franchise = JsonConvert.DeserializeObject<Franchise>(json);

                // Assert.
                franchise.ShouldNotBeNull();
                franchise.Id.ShouldBe(id);
            }
        }
    }
}