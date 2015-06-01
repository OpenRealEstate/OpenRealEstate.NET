using System;
using System.IO;
using System.Linq;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Services;
using OpenRealEstate.Services.RealEstateComAu;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class ListingHelpersFacts
    {
        public class CopyOverListingDataFacts
        {
            #region Residential
            
            [Fact]
            public void GivenANewCurrentResidentialListing_CopyOverListingData_ReturnsANewListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(reaXml);
                var newListing = result.Listings.First().Listing;
                newListing.UpdatedOn = DateTime.UtcNow;
                newListing.Title = "This is a new title";

                // Act.
                var listing = ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Title.ShouldBe(newListing.Title);
            }

            [Fact]
            public void GivenANewSoldResidentialListing_CopyOverListingData_ReturnsAnUpdatedListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
                var soldReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold.xml");
                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(soldReaXml);
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = (ResidentialListing)ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.StatusType.ShouldBe(StatusType.Sold);
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Pricing.SoldPrice.ShouldBe(580000);
                listing.Pricing.SoldPriceText.ShouldBe("$580,000");
                listing.Pricing.SoldOn.ShouldBe(new DateTime(2009, 01 ,10 ,12 ,30 ,00));
                listing.Title.ShouldBe(existingListing.Title); // Doesn't get copied over.
            }

            [Fact]
            public void GivenANewWithdrawnResidentialListing_CopyOverListingData_ReturnsAnUpdatedListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
                var soldReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Withdrawn.xml");
                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(soldReaXml);
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = (ResidentialListing)ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.StatusType.ShouldBe(StatusType.Withdrawn);
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Pricing.SoldPrice.ShouldBe(null);
                listing.Pricing.SoldPriceText.ShouldBe(null);
                listing.Pricing.SoldOn.ShouldBe(null);
                listing.Title.ShouldBe(existingListing.Title); // Doesn't get copied over.
            }

            [Fact]
            public void GivenANewOffMarketResidentialListing_CopyOverListingData_ReturnsAnUpdatedListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
                var soldReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-OffMarket.xml");
                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(soldReaXml);
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = (ResidentialListing)ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.StatusType.ShouldBe(StatusType.OffMarket);
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Pricing.SoldPrice.ShouldBe(null);
                listing.Pricing.SoldPriceText.ShouldBe(null);
                listing.Pricing.SoldOn.ShouldBe(null);
                listing.Title.ShouldBe(existingListing.Title); // Doesn't get copied over.
            }

            #endregion

            #region Rental

            [Fact]
            public void GivenANewCurrentRentalListing_CopyOverListingData_ReturnsANewListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Current.xml");

                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;
                
                result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = (RentalListing)ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Title.ShouldBe(newListing.Title);
                listing.Pricing.RentalPrice.ShouldBe(350);
                listing.Pricing.RentalPriceText.ShouldBe("$350");
                listing.Pricing.PaymentFrequencyType.ShouldBe(PaymentFrequencyType.Weekly);
            }

            [Fact]
            public void GivenANewLeasedRentalListing_CopyOverListingData_ReturnsAnUpdatedListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier(); 
                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Current.xml");
                var leasedReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Leased.xml");

                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(leasedReaXml);
                var newListing = result.Listings.First().Listing;
                //newListing.StatusType = StatusType.Leased;
                //newListing.UpdatedOn = DateTime.UtcNow;
                newListing.Title = "This is a new title";

                // Act.
                var listing = ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Title.ShouldBe(existingListing.Title);
            }

            [Fact]
            public void GivenANewWithdrawnRentalListing_CopyOverListingData_ReturnsAnUpdatedListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Current.xml");
                var soldReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Withdrawn.xml");
                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(soldReaXml);
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = (RentalListing)ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.StatusType.ShouldBe(StatusType.Withdrawn);
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Pricing.RentalPrice.ShouldBe(350);
                listing.Pricing.RentalPriceText.ShouldBe("$350");
                listing.Pricing.PaymentFrequencyType.ShouldBe(PaymentFrequencyType.Weekly);
                listing.Title.ShouldBe(existingListing.Title); // Doesn't get copied over.
            }

            [Fact]
            public void GivenANewOffMarketRentalListing_CopyOverListingData_ReturnsAnUpdatedListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Current.xml");
                var soldReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-OffMarket.xml");
                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(soldReaXml);
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = (RentalListing)ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.StatusType.ShouldBe(StatusType.OffMarket);
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Pricing.RentalPrice.ShouldBe(350);
                listing.Pricing.RentalPriceText.ShouldBe("$350");
                listing.Pricing.PaymentFrequencyType.ShouldBe(PaymentFrequencyType.Weekly);
                listing.Title.ShouldBe(existingListing.Title); // Doesn't get copied over.
            }

            [Fact]
            public void GivenANewLeasedResidentialListing_CopyOverListingData_ThrowsAnException()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(reaXml);
                var newListing = result.Listings.First().Listing;
                newListing.StatusType = StatusType.Leased;
                newListing.UpdatedOn = DateTime.UtcNow;
                newListing.Title = "This is a new title";

                // Act.
                var exception = Should.Throw<Exception>(() => ListingHelpers.CopyOverListingData(newListing, existingListing));

                // Assert.
                exception.Message.ShouldBe("Unhandled status type -or- incompatible status type with the listing type (ie. a residential listing cannot be leased ... only rental listings can be leased. New listing: 'Residential >> Agency: XNWXNW; Id: Residential-Current-ABCD1234' - 'Leased'. Existing listing: 'Residential >> Agency: XNWXNW; Id: Residential-Current-ABCD1234' - 'Current'");
            }

            #endregion

            #region Land

            [Fact]
            public void GivenANewCurrentdLandListing_CopyOverListingData_ReturnsANewListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current.xml");

                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Title.ShouldBe(newListing.Title);
                listing.StatusType.ShouldBe(StatusType.Current);
            }

            [Fact]
            public void GivenANewSoldLandListing_CopyOverListingData_ReturnsAnUpdatedListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current.xml");
                var leasedReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Sold.xml");

                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(leasedReaXml);
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Title.ShouldBe(existingListing.Title);
                listing.StatusType.ShouldBe(StatusType.Sold);
            }

            [Fact]
            public void GivenANewOffMarketLandListing_CopyOverListingData_ReturnsAnUpdatedListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current.xml");
                var offmarketReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-OffMarket.xml");

                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(offmarketReaXml);
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Title.ShouldBe(existingListing.Title);
                listing.StatusType.ShouldBe(StatusType.OffMarket);
            }

            [Fact]
            public void GivenANewWithdrawnLandListing_CopyOverListingData_ReturnsAnUpdatedListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current.xml");
                var offmarketReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Withdrawn.xml");

                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(offmarketReaXml);
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Title.ShouldBe(existingListing.Title);
                listing.StatusType.ShouldBe(StatusType.Withdrawn);
            }

            #endregion

            #region Rural

            [Fact]
            public void GivenANewCurrentdRuralListing_CopyOverListingData_ReturnsANewListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Current.xml");

                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Title.ShouldBe(newListing.Title);
                listing.StatusType.ShouldBe(StatusType.Current);
            }

            [Fact]
            public void GivenANewSoldRuralListing_CopyOverListingData_ReturnsAnUpdatedListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Current.xml");
                var leasedReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Sold.xml");

                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(leasedReaXml);
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Title.ShouldBe(existingListing.Title);
                listing.StatusType.ShouldBe(StatusType.Sold);
            }

            [Fact]
            public void GivenANewOffMarketRuralListing_CopyOverListingData_ReturnsAnUpdatedListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Current.xml");
                var offmarketReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-OffMarket.xml");

                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(offmarketReaXml);
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Title.ShouldBe(existingListing.Title);
                listing.StatusType.ShouldBe(StatusType.OffMarket);
            }

            [Fact]
            public void GivenANewWithdrawnRuralListing_CopyOverListingData_ReturnsAnUpdatedListing()
            {
                // Arrange.
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                var currentReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Current.xml");
                var offmarketReaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Withdrawn.xml");

                var result = reaXmlTransmorgrifier.ConvertTo(currentReaXml);
                var existingListing = result.Listings.First().Listing;

                result = reaXmlTransmorgrifier.ConvertTo(offmarketReaXml);
                var newListing = result.Listings.First().Listing;
                newListing.Title = "This is a new title";

                // Act.
                var listing = ListingHelpers.CopyOverListingData(newListing, existingListing);

                // Assert.
                listing.CreatedOn.ShouldBe(newListing.CreatedOn);
                listing.Title.ShouldBe(existingListing.Title);
                listing.StatusType.ShouldBe(StatusType.Withdrawn);
            }

            #endregion
        }
    }
}