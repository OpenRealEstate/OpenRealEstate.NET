using System.IO;
using System.Linq;
using OpenRealEstate.Services.RealEstateComAu;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class ResidentialListingFacts
    {
        public class CopyFacts
        {
            [Fact]
            public void GivenAnExistingAndNewListing_Copy_OnlyCopiesOverTheModifiedData()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
                var existingListing = reaXmlTransmorgrifier.ConvertTo(reaXml).Listings.First().Listing;
                existingListing.ClearAllIsModified();

                reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold.xml");
                var newListing = reaXmlTransmorgrifier.ConvertTo(reaXml).Listings.First().Listing;
                newListing.ClearAllIsModified();

                // Now lets add some new data.
                const string newValueText = "a";
                newListing.AgencyId = newValueText;
                newListing.Title = newValueText;
                newListing.Description = newValueText;
                
                // Act.
                existingListing.Copy(newListing);

                // Assert.
                existingListing.AgencyId.ShouldBe(newValueText);
                existingListing.Title.ShouldBe(newValueText);
                existingListing.Description.ShouldBe(newValueText);
            }
        }
    }
}