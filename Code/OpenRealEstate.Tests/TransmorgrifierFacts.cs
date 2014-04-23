using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenRealEstate.Core.Filters;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Services.RealEstate.com.au;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class TransmorgrifierFacts
    {
        public class ReaXmlTransmorgrifierFacts
        {
            [Fact]
            public void GivenAnReaXmlFile_Convert_ReturnsAListOfListings()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\Rea.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(25);

                var residentialListing = listings
                    .AsQueryable()
                    .WithId("1199536")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                residentialListing.ShouldNotBe(null);

                AssertResidentialListing(residentialListing);

                var rentalListing = listings
                    .AsQueryable()
                    .WithId("1426R")
                    .OfType<RentalListing>()
                    .SingleOrDefault();

                AssertRentalListing(rentalListing);

            }

            private static void AssertResidentialListing(ResidentialListing listing)
            {
                listing.AgencyId.ShouldBe("5140");
                listing.Id.ShouldBe("1199536");
                listing.StatusType.ShouldBe(StatusType.Current);
                listing.PropertyType.ShouldBe(PropertyType.House);

                listing.Address.IsStreetDisplayed.ShouldBe(true);
                listing.Address.StreetNumber.ShouldBe("30");
                listing.Address.Street.ShouldBe("Panorama Avenue");
                listing.Address.Suburb.ShouldBe("South West Rocks");
                listing.Address.State.ShouldBe("NSW");
                listing.Address.CountryIsoCode.ShouldBe("AU");
                listing.Address.Postcode.ShouldBe("2431");

                listing.Pricing.SalePrice.ShouldBe(329000m);
                listing.Pricing.SalePriceText.ShouldBeNullOrEmpty();
                listing.Pricing.IsUnderOffer.ShouldBe(true);

                listing.Inspections.Count.ShouldBe(1);
                listing.Inspections.First().OpensOn.ShouldBe(new DateTime(2014, 2, 21, 18, 15, 0));
                listing.Inspections.First().ClosesOn.ShouldBe(new DateTime(2014, 2, 21, 18, 30, 0));

                listing.Agents.Count.ShouldBe(1);
                var listingAgent = listing.Agents[0];
                listingAgent.Name.ShouldBe("Kelly Flanagan");
                listingAgent.Order.ShouldBe(1);
                listingAgent.Communications[0].CommunicationType.ShouldBe(CommunicationType.Email);
                listingAgent.Communications[0].Details.ShouldBe("receptionkf@sample.agency.1.com.au");
                listingAgent.Communications[1].CommunicationType.ShouldBe(CommunicationType.Landline);
                listingAgent.Communications[1].Details.ShouldBe("02 6562 3600");

                listing.Features.Bedrooms.ShouldBe(3);
                listing.Features.Bathrooms.ShouldBe(2);
                listing.Features.CarSpaces.ShouldBe(1);

                listing.Images.Count.ShouldBe(10);
                listing.Images[0].Order.ShouldBe(1);
                listing.Images[0].Url.ShouldBe("http://s3-ap-southeast-1.amazonaws.com/img.agentaccount.com/a4f063e1fe5b3468f54cd2a6c8c35292810d035d");

                listing.FloorPlans.Count.ShouldBe(2);
                listing.FloorPlans[0].Url.ShouldBe("http://img.somedomain.com/floorplan1.png");
                listing.FloorPlans[0].Order.ShouldBe(1);
                listing.FloorPlans[1].Url.ShouldBe("http://img.somedomain.com/floorplan2.png");
                listing.FloorPlans[0].Order.ShouldBe(1);

                var errors = new Dictionary<string, string>();
                listing.Validate(errors);
                errors.Count.ShouldBe(0);
            }

            private static void AssertRentalListing(RentalListing listing)
            {
                listing.AgencyId.ShouldBe("97");
                listing.Id.ShouldBe("1426R");
                listing.StatusType.ShouldBe(StatusType.Current);
                listing.PropertyType.ShouldBe(PropertyType.Townhouse);

                listing.Address.IsStreetDisplayed.ShouldBe(true);
                listing.Address.StreetNumber.ShouldBe("5/35");
                listing.Address.Street.ShouldBe("Ross Street");
                listing.Address.Suburb.ShouldBe("ALLENSTOWN");
                listing.Address.State.ShouldBe("QLD");
                listing.Address.CountryIsoCode.ShouldBe("AU");
                listing.Address.Postcode.ShouldBe("4700");

                listing.Pricing.RentalPrice.ShouldBe(330);
                listing.Pricing.RentalPriceText.ShouldBe("$330 / Wk");
                listing.Pricing.AvailableOn.ShouldBe(new DateTime(2014, 02, 20));
                listing.Pricing.Bond.ShouldBe(1320);

                listing.Inspections.ShouldBe(null);

                listing.Agents.Count.ShouldBe(1);
                var listingAgent = listing.Agents[0];
                listingAgent.Name.ShouldBe("LJ Hooker Rockhampton");
                listingAgent.Order.ShouldBe(1);
                listingAgent.Communications[0].CommunicationType.ShouldBe(CommunicationType.Email);
                listingAgent.Communications[0].Details.ShouldBe("rockhampton@sample.agency.1.com.au");
                listingAgent.Communications[1].CommunicationType.ShouldBe(CommunicationType.Landline);
                listingAgent.Communications[1].Details.ShouldBe("(07) 4922 2244");

                listing.Features.Bedrooms.ShouldBe(3);
                listing.Features.Bathrooms.ShouldBe(1);
                listing.Features.CarSpaces.ShouldBe(0);

                listing.Images.Count.ShouldBe(8);
                listing.Images[0].Order.ShouldBe(1);
                listing.Images[0].Url.ShouldBe("http://images.ljhooker.com.au/RentalProperty/Pictures/01582296.jpg");

                listing.FloorPlans.ShouldBe(null);

                var errors = new Dictionary<string, string>();
                listing.Validate(errors);
                errors.Count.ShouldBe(0);
            }
        }
    }
}