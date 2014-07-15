using System.IO;
using Newtonsoft.Json;
using OpenRealEstate.Core.Models;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class FranchiseFacts
    {
        [Fact]
        public void GivenAFranchiseJsonFileActiveSampleFranchise1_Deserialization_ValidatesAFranchise()
        {
            // Arrange.
            var json = File.ReadAllText("Sample Data\\Franchises\\Active-Sample.Franchise.1.json");

            // Act.
            var franchise = JsonConvert.DeserializeObject<Franchise>(json);

            // Assert.
            franchise.ShouldNotBe(null);
            franchise.Id.ShouldBe("sample.franchise.1");
        }

        [Fact]
        public void GivenAFranchiseJsonFileActiveSampleFranchise1BadData_Deserialization_DoesNotValidateAFranchise()
        {
            // Arrange.
            var json = File.ReadAllText("Sample Data\\Franchises\\Active-Sample.Franchise.1-BadData.json");

            // Act.
            var franchise = JsonConvert.DeserializeObject<Franchise>(json);

            // Assert.
            franchise.ShouldNotBe(null);
            franchise.Id.ShouldBe(null); // Missing data.
        }
    }
}