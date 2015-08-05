using System;
using OpenRealEstate.Core.Models;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class AggregateRootFacts
    {
        public class CopyOverNewDataFacts
        {
            [Fact]
            public void GivenAnExistingListing_CopyOverNewData_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.ResidentialListingFromFile as AggregateRoot;
                sourceListing.Id = "2140F1E6-EF8B-45D4-82FB-90940A3F1D90";
                sourceListing.UpdatedOn = DateTime.UtcNow;

                var destinationListing = HelperUtilities.ResidentialListingFromFile as AggregateRoot;
                ;

                // Act.
                destinationListing.CopyOverNewData(sourceListing);

                // Assert.
                destinationListing.Id.ShouldBe(sourceListing.Id);
                destinationListing.IsIdModified.ShouldBe(false);
                destinationListing.UpdatedOn.ShouldBe(sourceListing.UpdatedOn);
                destinationListing.IsUpdatedOnModified.ShouldBe(false);
            }
        }
    }
}