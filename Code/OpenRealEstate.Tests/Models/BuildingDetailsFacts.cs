using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class BuildingDetailsFacts
    {
        public class AreaIsModifiedFacts
        {
            [Fact]
            public void GivenABuildingDetailsWithAnExistingAreaAndTheAreaValueIsUpdated_AreaIsModified_ReturnsTrue()
            {
                // Arrange.
                var listing = TestHelperUtilities.ResidentialListingFromFile();
                listing.BuildingDetails.IsModified.ShouldBe(false);
                const decimal value = 2;

                // Act.
                listing.BuildingDetails.Area.Value = value;

                // Assert.
                listing.BuildingDetails.IsModified.ShouldBe(true);
            }
        }
    }
}