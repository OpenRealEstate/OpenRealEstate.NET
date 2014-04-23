using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OpenRealEstate.Core.Models;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class AgencyFacts
    {
        [Fact]
        public void GivenAnAgencyJsonFileActiveSampleAgency1_Deserialization_ValidatesAnAgency()
        {
            // Arrange.
            var json = File.ReadAllText("Sample Data\\Agencies\\Active-Sample.Agency.1.json");
 
            // Act.
            var agency = JsonConvert.DeserializeObject<Agency>(json);

            // Assert.
            agency.ShouldNotBe(null);
            agency.Id.ShouldBe("sample.agency.1");
            var validationErrors = new Dictionary<string, string>();
            agency.Validate(validationErrors);
            validationErrors.ShouldBeEmpty();
        }

        [Fact]
        public void GivenAnAgencyJsonFileActiveSampleAgency1BadData_Deserialization_DoesNotValidateAnAgency()
        {
            // Arrange.
            var json = File.ReadAllText("Sample Data\\Agencies\\Active-Sample.Agency.1-BadData.json");

            // Act.
            var agency = JsonConvert.DeserializeObject<Agency>(json);

            // Assert.
            agency.ShouldNotBe(null);
            agency.Id.ShouldBe("sample.agency.1");
            var validationErrors = new Dictionary<string, string>();
            agency.Validate(validationErrors);
            validationErrors.ShouldNotBe(null);
            validationErrors.First().Key.ShouldBe("Name");
            validationErrors.First().Value.ShouldBe("A name is required.");
        }
    }
}