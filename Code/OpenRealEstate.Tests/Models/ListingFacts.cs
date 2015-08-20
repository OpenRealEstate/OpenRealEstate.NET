﻿using System.Collections.Generic;
using System.Linq;
using OpenRealEstate.Core.Models;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class ListingFacts
    {
        public class CopyOverNewDataFacts
        {
            [Fact]
            public void GivenAnExistingListingAndANewListingWithEverythingModified_CopyOverNewData_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.ResidentialListingFromFile(false) as Listing;
                
                var destinationListing = HelperUtilities.ResidentialListing() as Listing;

                // Act.
                destinationListing.CopyOverNewData(sourceListing);

                // Assert.
                destinationListing.AgencyId.ShouldBe(sourceListing.AgencyId);
                destinationListing.IsAgencyIdModified.ShouldBe(true);
                destinationListing.CreatedOn.ShouldBe(sourceListing.CreatedOn);
                destinationListing.IsCreatedOnModified.ShouldBe(true);
                destinationListing.Description.ShouldBe(sourceListing.Description);
                destinationListing.IsDescriptionModified.ShouldBe(true);
                destinationListing.Id.ShouldBe(sourceListing.Id);
                destinationListing.IsIdModified.ShouldBe(true);
                destinationListing.StatusType.ShouldBe(sourceListing.StatusType);
                destinationListing.IsStatusTypeModified.ShouldBe(true);
                destinationListing.Title.ShouldBe(sourceListing.Title);
                destinationListing.IsTitleModified.ShouldBe(true);

                destinationListing.Address.CountryIsoCode.ShouldBe(sourceListing.Address.CountryIsoCode);
                destinationListing.Address.IsCountryIsoCodeModified.ShouldBe(true);
                destinationListing.Address.IsStreetDisplayed.ShouldBe(sourceListing.Address.IsStreetDisplayed);
                destinationListing.Address.IsIsStreetDisplayedModified.ShouldBe(true);
                destinationListing.Address.Latitude.ShouldBe(sourceListing.Address.Latitude);
                destinationListing.Address.IsLatitudeModified.ShouldBe(true);
                destinationListing.Address.Longitude.ShouldBe(sourceListing.Address.Longitude);
                destinationListing.Address.IsLongitudeModified.ShouldBe(true);
                destinationListing.Address.Municipality.ShouldBe(sourceListing.Address.Municipality);
                destinationListing.Address.IsMunicipalityModified.ShouldBe(true);
                destinationListing.Address.Postcode.ShouldBe(sourceListing.Address.Postcode);
                destinationListing.Address.IsPostcodeModified.ShouldBe(true);
                destinationListing.Address.State.ShouldBe(sourceListing.Address.State);
                destinationListing.Address.IsStateModified.ShouldBe(true);
                destinationListing.Address.Street.ShouldBe(sourceListing.Address.Street);
                destinationListing.Address.IsStreetModified.ShouldBe(true);
                destinationListing.Address.StreetNumber.ShouldBe(sourceListing.Address.StreetNumber);
                destinationListing.Address.IsStreetNumberModified.ShouldBe(true);
                destinationListing.Address.Suburb.ShouldBe(sourceListing.Address.Suburb);
                destinationListing.Address.IsSuburbModified.ShouldBe(true);
                destinationListing.IsAddressModified.ShouldBe(true);

                for (int i = 0; i < destinationListing.Agents.Count; i++)
                {
                    destinationListing.Agents[i].Name.ShouldBe(sourceListing.Agents[i].Name);
                    destinationListing.Agents[i].IsNameModified.ShouldBe(true);
                    destinationListing.Agents[i].Order.ShouldBe(sourceListing.Agents[i].Order);
                    destinationListing.Agents[i].IsOrderModified.ShouldBe(true);
                    for (int j = 0; j < destinationListing.Agents[i].Communications.Count; j++)
                    {
                        destinationListing.Agents[i].Communications[j].CommunicationType.ShouldBe(
                            sourceListing.Agents[i].Communications[j].CommunicationType);
                        destinationListing.Agents[i].Communications[j].IsCommunicationTypeModified.ShouldBe(true);
                        destinationListing.Agents[i].Communications[j].Details.ShouldBe(
                            sourceListing.Agents[i].Communications[j].Details);
                        destinationListing.Agents[i].Communications[j].IsDetailsModified.ShouldBe(true);
                    }
                    destinationListing.Agents[i].IsCommunicationsModified.ShouldBe(true);
                    destinationListing.IsAgentsModified.ShouldBe(true);
                }

                destinationListing.Features.Bathrooms.ShouldBe(sourceListing.Features.Bathrooms);
                destinationListing.Features.IsBathroomsModified.ShouldBe(true);
                destinationListing.Features.Bedrooms.ShouldBe(sourceListing.Features.Bedrooms);
                destinationListing.Features.IsBedroomsModified.ShouldBe(true);
                destinationListing.Features.Carports.ShouldBe(sourceListing.Features.Carports);
                destinationListing.Features.IsCarportsModified.ShouldBe(true);
                destinationListing.Features.Ensuites.ShouldBe(sourceListing.Features.Ensuites);
                destinationListing.Features.IsEnsuitesModified.ShouldBe(true);
                destinationListing.Features.Garages.ShouldBe(sourceListing.Features.Garages);
                destinationListing.Features.IsGaragesModified.ShouldBe(true);
                destinationListing.Features.LivingAreas.ShouldBe(sourceListing.Features.LivingAreas);
                destinationListing.Features.IsLivingAreasModified.ShouldBe(true);
                destinationListing.Features.OpenSpaces.ShouldBe(sourceListing.Features.OpenSpaces);
                destinationListing.Features.IsOpenSpacesModified.ShouldBe(true);
                destinationListing.Features.Tags.SetEquals(sourceListing.Features.Tags);
                destinationListing.Features.IsTagsModified.ShouldBe(true);
                destinationListing.Features.Toilets.ShouldBe(sourceListing.Features.Toilets);
                destinationListing.Features.IsToiletsModified.ShouldBe(true);
                destinationListing.IsFeaturesModified.ShouldBe(true);

                HelperUtilities.AssertMediaItems(destinationListing.FloorPlans, sourceListing.FloorPlans);
                destinationListing.IsFloorPlansModified.ShouldBe(true);

                HelperUtilities.AssertMediaItems(destinationListing.Images, sourceListing.Images);
                destinationListing.IsImagesModified.ShouldBe(true);

                for (int i = 0; i < destinationListing.Inspections.Count; i++)
                {
                    destinationListing.Inspections[0].OpensOn.ShouldBe(sourceListing.Inspections[0].OpensOn);
                    destinationListing.Inspections[0].IsOpensOnModified.ShouldBe(true);
                    destinationListing.Inspections[0].ClosesOn.ShouldBe(sourceListing.Inspections[0].ClosesOn);
                    destinationListing.Inspections[0].IsClosesOnModified.ShouldBe(true);
                }
                destinationListing.IsInspectionsModified.ShouldBe(true);

                destinationListing.LandDetails.Area.Type.ShouldBe(sourceListing.LandDetails.Area.Type);
                destinationListing.LandDetails.Area.IsTypeModified.ShouldBe(true);
                destinationListing.LandDetails.Area.Value.ShouldBe(sourceListing.LandDetails.Area.Value);
                destinationListing.LandDetails.Area.IsValueModified.ShouldBe(true);
                destinationListing.LandDetails.CrossOver.ShouldBe(sourceListing.LandDetails.CrossOver);
                destinationListing.LandDetails.IsCrossOverModified.ShouldBe(true);
                for (int i = 0; i < destinationListing.LandDetails.Depths.Count; i++)
                {
                    destinationListing.LandDetails.Depths[i].Type.ShouldBe(sourceListing.LandDetails.Depths[i].Type);
                    destinationListing.LandDetails.Depths[i].IsTypeModified.ShouldBe(true);
                    destinationListing.LandDetails.Depths[i].Value.ShouldBe(sourceListing.LandDetails.Depths[i].Value);
                    destinationListing.LandDetails.Depths[i].IsValueModified.ShouldBe(true);
                    destinationListing.LandDetails.Depths[i].Side.ShouldBe(sourceListing.LandDetails.Depths[i].Side);
                    destinationListing.LandDetails.Depths[i].IsSideModified.ShouldBe(true);
                }
                destinationListing.LandDetails.IsDepthsModified.ShouldBe(true);
                destinationListing.LandDetails.Frontage.Type.ShouldBe(sourceListing.LandDetails.Frontage.Type);
                destinationListing.LandDetails.Frontage.IsTypeModified.ShouldBe(true);
                destinationListing.LandDetails.Frontage.Value.ShouldBe(sourceListing.LandDetails.Frontage.Value);
                destinationListing.LandDetails.IsFrontageModified.ShouldBe(true);
                destinationListing.IsLandDetailsModified.ShouldBe(true);

                for (int i = 0; i < destinationListing.Links.Count; i++)
                {
                    destinationListing.Links[i].ShouldBe(sourceListing.Links[i]);
                }
                destinationListing.IsLinksModified.ShouldBe(true);

                HelperUtilities.AssertMediaItems(destinationListing.Videos, sourceListing.Videos);
                destinationListing.IsImagesModified.ShouldBe(true);
            }

            [Fact]
            public void GivenAnExistingListingAndANewListingWithANullValues_CopyOverNewData_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.ResidentialListingFromFile();
                sourceListing.Agents = null;
                sourceListing.Address = null;
                sourceListing.Features = null;
                sourceListing.FloorPlans = null;
                sourceListing.Images = null;
                sourceListing.Inspections = null;
                sourceListing.LandDetails = null;
                sourceListing.Links = null;
                sourceListing.Videos = null;

                var destinationListing = HelperUtilities.ResidentialListingFromFile();

                // Act.
                destinationListing.CopyOverNewData(sourceListing);

                // Assert.
                destinationListing.Agents.ShouldBe(null);
                destinationListing.IsAgentsModified.ShouldBe(true);
                destinationListing.Address.ShouldBe(null);
                destinationListing.IsAddressModified.ShouldBe(true);
                destinationListing.Features.ShouldBe(null);
                destinationListing.IsFeaturesModified.ShouldBe(true);
                destinationListing.FloorPlans.ShouldBe(null);
                destinationListing.IsFloorPlansModified.ShouldBe(true);
                destinationListing.Images.ShouldBe(null);
                destinationListing.IsImagesModified.ShouldBe(true);
                destinationListing.Inspections.ShouldBe(null);
                destinationListing.IsInspectionsModified.ShouldBe(true);
                destinationListing.LandDetails.ShouldBe(null);
                destinationListing.IsLandDetailsModified.ShouldBe(true);
                destinationListing.Links.ShouldBe(null);
                destinationListing.IsLinksModified.ShouldBe(true);
                destinationListing.Videos.ShouldBe(null);
                destinationListing.IsVideosModified.ShouldBe(true);
            }

            [Fact]
            public void GivenAnExistingListingAndChangingAnImage_CopyOverNewData_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.ResidentialListingFromFile();
                var destinationListing = HelperUtilities.ResidentialListingFromFile();

                sourceListing.Images = new List<Media>
                {
                    new Media
                    {
                        Url = "http://a.b/c",
                        Order = 1,
                        Tag = "hi!"
                    }
                };

                // Act.
                // NOTE: this will just copy over the 1 media item.
                destinationListing.CopyOverNewData(sourceListing);

                // Assert.
                destinationListing.Images.First().Url.ShouldBe(sourceListing.Images.First().Url);
                destinationListing.IsImagesModified.ShouldBe(true);

                // Now lets change the source and the destination should _not_ change.
                sourceListing.Images.First().Url = "https://1.2.3.4/5";
                destinationListing.Images.First().Url.ShouldNotBe(sourceListing.Images.First().Url);
            }

            [Fact]
            public void GivenAnExistingListingAndChangingAnAgentAfterwards_CopyOverNewData_TheAgentsAreNotBothUpdated()
            {
                // Arrange.
                var sourceListing = HelperUtilities.ResidentialListing(false);
                var destinationListing = HelperUtilities.ResidentialListingFromFile();

                // Act.
                destinationListing.CopyOverNewData(sourceListing);
                destinationListing.Agents.First().Name = "I'm a Princess";

                // Assert.
                destinationListing.Agents.First().Name.ShouldNotBe(sourceListing.Agents.First().Name);
            }
        }
    }
}