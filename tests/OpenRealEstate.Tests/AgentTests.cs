using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OpenRealEstate.Core;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class AgentTests
    {
        public class DeserializationTests
        {
            [Fact]
            public void GivenAnAgentJsonFileActiveJaneSmith_Deserialization_ValidatesAnAgent()
            {
                // Arrange.
                var json = File.ReadAllText("Sample Data\\Agents\\Active-Jane.Smith.json");

                // Act.
                var agent = JsonConvert.DeserializeObject<Agent>(json);

                // Assert.
                agent.ShouldNotBeNull();
                agent.Id.ShouldBe("Jane.Smith");
                var validationErrors = new Dictionary<string, string>();
                agent.Validate(validationErrors);
                validationErrors.ShouldBeEmpty();
            }

            [Fact]
            public void GivenAnAgentJsonFileActiveJaneSmithBadData_Deserialization_DoesNotValidateAnAgent()
            {
                // Arrange.
                var json = File.ReadAllText("Sample Data\\Agents\\Active-Jane.Smith-BadData.json");

                // Act.
                var agent = JsonConvert.DeserializeObject<Agent>(json);

                // Assert.
                agent.ShouldNotBeNull();
                agent.Id.ShouldBe("Jane.Smith");
                var validationErrors = new Dictionary<string, string>();
                agent.Validate(validationErrors);
                validationErrors.ShouldNotBeNull();
                validationErrors["Name"].ShouldBe("A name is required. eg. Jane Smith.");
                validationErrors["AgencyIds"].ShouldBe("At least one AgencyId is requires where this Agent works at.");
            }
        }
    }
}