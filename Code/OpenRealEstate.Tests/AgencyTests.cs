using System.IO;
using Newtonsoft.Json;
using OpenRealEstate.Core.Models;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class AgencyTests
    {
        public class DeserializationTests
        {
            [Theory]
            [InlineData("Sample Data\\Agencies\\Active-Sample.Agency.1.json", "sample.agency.1")]
            [InlineData("Sample Data\\Agencies\\Active-Sample.Agency.1-BadData.json", null)]
            public void GivenAnAgencyJsonFileActiveSampleAgency1_Deserialization_ValidatesAnAgency(string path,
                string id)
            {
                // Arrange.
                var json = File.ReadAllText(path);

                // Act.
                var Office = JsonConvert.DeserializeObject<Offices>(json);

                // Assert.
                Office.ShouldNotBeNull();
                Office.Id.ShouldBe(id);
            }
        }
    }
}