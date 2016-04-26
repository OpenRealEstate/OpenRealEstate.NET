using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Land;
using OpenRealEstate.FakeData;
using OpenRealEstate.Services;
using OpenRealEstate.Services.RealEstateComAu;
using Shouldly;
using Xunit;
using LandRuralCategoryType = OpenRealEstate.Core.Land.CategoryType;

namespace OpenRealEstate.Tests.Transmorgrifiers.REA
{
    public class ParseLandTests
    {
        private const string FakeDataFolder = "Sample Data\\Transmorgrifiers\\REA\\Land\\";

        private static LandListing CreateAFakeEmptyLandListing(string id)
        {
            id.ShouldNotBeNullOrWhiteSpace();

            return new LandListing
            {
                AgencyId = "XNWXNW",
                Id = id,
                CreatedOn = new DateTime(2009, 1, 1, 12, 30, 0),
                UpdatedOn = new DateTime(2009, 1, 1, 12, 30, 0)
            };
        }

        private static void AssertLandListing(ParsedResult result,
            LandListing expectedListing)
        {
            result.ShouldNotBeNull();
            result.Listings.Count.ShouldBe(1);
            result.Errors.Count.ShouldBe(0);
            result.UnhandledData.Count.ShouldBe(0);
            LandListingAssertHelpers.AssertLandListing(result.Listings.First().Listing as LandListing,
                expectedListing);
        }

        [Theory]
        [InlineData("REA-Land-Current.xml")]
        [InlineData("REA-Segment-Land-Current.xml")]
        public void GivenTheFileREALandCurrent_Parse_ReturnsALandAvailableListing(string fileName)
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeLandListing();
            var reaXml = File.ReadAllText(FakeDataFolder + fileName);
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertLandListing(result, expectedListing);
        }

        [Theory]
        [InlineData("REA-Land-Sold.xml", "$85,000")]
        [InlineData("REA-Land-Sold-DisplayPriceIsNo.xml", null)]
        public void GivenAnREALandSoldFile_Parse_ReturnsARemovedListing(string fileName, string soldPriceText)
        {
            // Arrange.
            var expectedListing = CreateAFakeEmptyLandListing("Land-Sold-ABCD1234");
            expectedListing.StatusType = StatusType.Sold;
            expectedListing.Pricing = new SalePricing
            {
                SoldPrice = 85000,
                SoldPriceText = soldPriceText,
                SoldOn = new DateTime(2009, 1, 10, 12, 30, 00)
            };
            var reaXml = File.ReadAllText(FakeDataFolder + fileName);
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertLandListing(result, expectedListing);
        }

        [Theory]
        [InlineData("REA-Land-Withdrawn.xml", "Land-Withdrawn-ABCD1234")]
        [InlineData("REA-Land-OffMarket.xml", "Land-OffMarket-ABCD1234")]
        [InlineData("REA-Land-Deleted.xml", "Land-Deleted-ABCD1234")]
        public void GivenAnReaLandFileThatRepresentsARemovedListing_Parse_ReturnsARemovedListing(
            string fileName,
            string id)
        {
            // Arrange.
            var expectedListing = CreateAFakeEmptyLandListing(id);
            expectedListing.StatusType = StatusType.Removed;
            var reaXml = File.ReadAllText(FakeDataFolder + fileName);
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertLandListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREALandCurrentIncompleteLandDetails_Parse_ReturnsALandAvailableListing()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeLandListing();
            expectedListing.LandDetails.CrossOver = null;
            expectedListing.LandDetails.Depths = new List<Depth>().AsReadOnly();
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Land-Current-IncompleteLandDetails.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertLandListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREALandCurrentMissingLandCategory_Parse_ReturnsALandAvailableListing()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeLandListing();
            expectedListing.CategoryType = LandRuralCategoryType.Unknown;
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Land-Current-MissingLandCategory.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertLandListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREALandCurrentWithASubNumberButNoStreetNumber_Parse_ReturnsALandAvailableListing()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeLandListing();
            expectedListing.Address.StreetNumber = "12";
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Land-Current-WithASubNumberButNoStreetNumber.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertLandListing(result, expectedListing);
        }
    }
}