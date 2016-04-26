using System;
using System.IO;
using System.Linq;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Rural;
using OpenRealEstate.FakeData;
using OpenRealEstate.Services;
using OpenRealEstate.Services.RealEstateComAu;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Transmorgrifiers.REA
{
    public class ParseRuralTests
    {
        private const string FakeDataFolder = "Sample Data\\Transmorgrifiers\\REA\\Rural\\";

        private static RuralListing CreateAFakeEmptyRuralListing(string id)
        {
            id.ShouldNotBeNullOrWhiteSpace();

            return new RuralListing
            {
                AgencyId = "XNWXNW",
                Id = id,
                CreatedOn = new DateTime(2009, 1, 1, 12, 30, 0),
                UpdatedOn = new DateTime(2009, 1, 1, 12, 30, 0)
            };
        }

        private static void AssertRuralListing(ParsedResult result,
            RuralListing expectedListing)
        {
            result.ShouldNotBeNull();
            result.Listings.Count.ShouldBe(1);
            result.Errors.Count.ShouldBe(0);
            result.UnhandledData.Count.ShouldBe(0);
            RuralListingAssertHelpers.AssertRuralListing(result.Listings.First().Listing as RuralListing,
                expectedListing);
        }

        [Theory]
        [InlineData("REA-Rural-Current.xml")]
        [InlineData("REA-Segment-Rural-Current.xml")]
        public void GivenTheFileREARuralCurrent_Parse_ReturnsARuralAvailableListing(string fileName)
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeRuralListing();
            var reaXml = File.ReadAllText(FakeDataFolder + fileName);
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertRuralListing(result, expectedListing);
        }

        [Theory]
        [InlineData("REA-Rural-Withdrawn.xml", "Rural-Withdrawn-ABCD1234")]
        [InlineData("REA-Rural-OffMarket.xml", "Rural-OffMarket-ABCD1234")]
        [InlineData("REA-Rural-Deleted.xml", "Rural-Deleted-ABCD1234")]
        public void GivenAnReaRuralFileThatRepresentsARemovedListing_Parse_ReturnsARemovedListing(
            string fileName,
            string id)
        {
            // Arrange.
            var expectedListing = CreateAFakeEmptyRuralListing(id);
            expectedListing.StatusType = StatusType.Removed;
            var reaXml = File.ReadAllText(FakeDataFolder + fileName);
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertRuralListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREARuralSoldDisplayPriceisNo_Parse_ReturnsARuralSoldListing()
        {
            // Arrange.
            var expectedListing = CreateAFakeEmptyRuralListing("Rural-Sold-ABCD1234");
            expectedListing.StatusType = StatusType.Sold;
            expectedListing.Pricing = new SalePricing
            {
                SoldOn = new DateTime(2009, 1, 10, 12, 30, 00),
                SoldPrice = 85000
            };
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Rural-Sold-DisplayPriceisNo.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertRuralListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREARuralSolder_Parse_ReturnsARemovedListing()
        {
            // Arrange.
            var expectedListing = CreateAFakeEmptyRuralListing("Rural-Sold-ABCD1234");
            expectedListing.StatusType = StatusType.Sold;
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Rural-Sold.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertRuralListing(result, expectedListing);
        }
    }
}