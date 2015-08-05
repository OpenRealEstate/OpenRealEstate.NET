using System.Collections.Generic;
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
                var sourceListing = HelperUtilities.ResidentialListing as Listing;
                var destinationListing = HelperUtilities.ResidentialListingFromFile as Listing;

                // Act.
                destinationListing.CopyOverNewData(sourceListing);

                // Assert.
                destinationListing.AgencyId.ShouldBe(sourceListing.AgencyId);
                destinationListing.IsAgencyIdModified.ShouldBe(false);
                destinationListing.CreatedOn.ShouldBe(sourceListing.CreatedOn);
                destinationListing.IsCreatedOnModified.ShouldBe(false);
                destinationListing.Description.ShouldBe(sourceListing.Description);
                destinationListing.IsDescriptionModified.ShouldBe(false);
                destinationListing.Id.ShouldBe(sourceListing.Id);
                destinationListing.IsIdModified.ShouldBe(false);
                destinationListing.StatusType.ShouldBe(sourceListing.StatusType);
                destinationListing.IsStatusTypeModified.ShouldBe(false);
                destinationListing.Title.ShouldBe(sourceListing.Title);
                destinationListing.IsTitleModified.ShouldBe(false);

                destinationListing.Address.CountryIsoCode.ShouldBe(sourceListing.Address.CountryIsoCode);
                destinationListing.Address.IsCountryIsoCodeModified.ShouldBe(false);
                destinationListing.Address.IsStreetDisplayed.ShouldBe(sourceListing.Address.IsStreetDisplayed);
                destinationListing.Address.IsIsStreetDisplayedModified.ShouldBe(false);
                destinationListing.Address.Latitude.ShouldBe(sourceListing.Address.Latitude);
                destinationListing.Address.IsLatitudeModified.ShouldBe(false);
                destinationListing.Address.Longitude.ShouldBe(sourceListing.Address.Longitude);
                destinationListing.Address.IsLongitudeModified.ShouldBe(false);
                destinationListing.Address.Municipality.ShouldBe(sourceListing.Address.Municipality);
                destinationListing.Address.IsMunicipalityModified.ShouldBe(false);
                destinationListing.Address.Postcode.ShouldBe(sourceListing.Address.Postcode);
                destinationListing.Address.IsPostcodeModified.ShouldBe(false);
                destinationListing.Address.State.ShouldBe(sourceListing.Address.State);
                destinationListing.Address.IsStateModified.ShouldBe(false);
                destinationListing.Address.Street.ShouldBe(sourceListing.Address.Street);
                destinationListing.Address.IsStreetModified.ShouldBe(false);
                destinationListing.Address.StreetNumber.ShouldBe(sourceListing.Address.StreetNumber);
                destinationListing.Address.IsStreetNumberModified.ShouldBe(false);
                destinationListing.Address.Suburb.ShouldBe(sourceListing.Address.Suburb);
                destinationListing.Address.IsSuburbModified.ShouldBe(false);
                destinationListing.IsAddressModified.ShouldBe(false);

                for (int i = 0; i < destinationListing.Agents.Count; i++)
                {
                    destinationListing.Agents[i].Name.ShouldBe(sourceListing.Agents[i].Name);
                    destinationListing.Agents[i].IsNameModified.ShouldBe(false);
                    destinationListing.Agents[i].Order.ShouldBe(sourceListing.Agents[i].Order);
                    destinationListing.Agents[i].IsOrderModified.ShouldBe(false);
                    for (int j = 0; j < destinationListing.Agents[i].Communications.Count; j++)
                    {
                        destinationListing.Agents[i].Communications[j].CommunicationType.ShouldBe(
                            sourceListing.Agents[i].Communications[j].CommunicationType);
                        destinationListing.Agents[i].Communications[j].IsCommunicationTypeModified.ShouldBe(false);
                        destinationListing.Agents[i].Communications[j].Details.ShouldBe(
                            sourceListing.Agents[i].Communications[j].Details);
                        destinationListing.Agents[i].Communications[j].IsDetailsModified.ShouldBe(false);
                    }
                    destinationListing.Agents[i].IsCommunicationsModified.ShouldBe(false);
                    destinationListing.IsAgentsModified.ShouldBe(false);
                }

                destinationListing.Features.Bathrooms.ShouldBe(sourceListing.Features.Bathrooms);
                destinationListing.Features.IsBathroomsModified.ShouldBe(false);
                destinationListing.Features.Bedrooms.ShouldBe(sourceListing.Features.Bedrooms);
                destinationListing.Features.IsBedroomsModified.ShouldBe(false);
                destinationListing.Features.Carports.ShouldBe(sourceListing.Features.Carports);
                destinationListing.Features.IsCarportsModified.ShouldBe(false);
                destinationListing.Features.Ensuites.ShouldBe(sourceListing.Features.Ensuites);
                destinationListing.Features.IsEnsuitesModified.ShouldBe(false);
                destinationListing.Features.Garages.ShouldBe(sourceListing.Features.Garages);
                destinationListing.Features.IsGaragesModified.ShouldBe(false);
                destinationListing.Features.LivingAreas.ShouldBe(sourceListing.Features.LivingAreas);
                destinationListing.Features.IsLivingAreasModified.ShouldBe(false);
                destinationListing.Features.OpenSpaces.ShouldBe(sourceListing.Features.OpenSpaces);
                destinationListing.Features.IsOpenSpacesModified.ShouldBe(false);
                destinationListing.Features.Tags.SetEquals(sourceListing.Features.Tags);
                destinationListing.Features.IsTagsModified.ShouldBe(false);
                destinationListing.Features.Toilets.ShouldBe(sourceListing.Features.Toilets);
                destinationListing.Features.IsToiletsModified.ShouldBe(false);
                destinationListing.IsFeaturesModified.ShouldBe(false);

                HelperUtilities.AssertMediaItems(destinationListing.FloorPlans, sourceListing.FloorPlans);
                destinationListing.IsFloorPlansModified.ShouldBe(false);

                HelperUtilities.AssertMediaItems(destinationListing.Images, sourceListing.Images);
                destinationListing.IsImagesModified.ShouldBe(false);

                for (int i = 0; i < destinationListing.Inspections.Count; i++)
                {
                    destinationListing.Inspections[0].OpensOn.ShouldBe(sourceListing.Inspections[0].OpensOn);
                    destinationListing.Inspections[0].IsOpensOnModified.ShouldBe(false);
                    destinationListing.Inspections[0].ClosesOn.ShouldBe(sourceListing.Inspections[0].ClosesOn);
                    destinationListing.Inspections[0].IsClosesOnModified.ShouldBe(false);
                }
                destinationListing.IsInspectionsModified.ShouldBe(false);

                destinationListing.LandDetails.Area.Type.ShouldBe(sourceListing.LandDetails.Area.Type);
                destinationListing.LandDetails.Area.IsTypeModified.ShouldBe(false);
                destinationListing.LandDetails.Area.Value.ShouldBe(sourceListing.LandDetails.Area.Value);
                destinationListing.LandDetails.Area.IsValueModified.ShouldBe(false);
                destinationListing.LandDetails.CrossOver.ShouldBe(sourceListing.LandDetails.CrossOver);
                destinationListing.LandDetails.IsCrossOverModified.ShouldBe(false);
                for (int i = 0; i < destinationListing.LandDetails.Depths.Count; i++)
                {
                    destinationListing.LandDetails.Depths[i].Type.ShouldBe(sourceListing.LandDetails.Depths[i].Type);
                    destinationListing.LandDetails.Depths[i].IsTypeModified.ShouldBe(false);
                    destinationListing.LandDetails.Depths[i].Value.ShouldBe(sourceListing.LandDetails.Depths[i].Value);
                    destinationListing.LandDetails.Depths[i].IsValueModified.ShouldBe(false);
                    destinationListing.LandDetails.Depths[i].Side.ShouldBe(sourceListing.LandDetails.Depths[i].Side);
                    destinationListing.LandDetails.Depths[i].IsSideModified.ShouldBe(false);
                }
                destinationListing.LandDetails.IsDepthsModified.ShouldBe(false);
                destinationListing.LandDetails.Frontage.Type.ShouldBe(sourceListing.LandDetails.Frontage.Type);
                destinationListing.LandDetails.Frontage.IsTypeModified.ShouldBe(false);
                destinationListing.LandDetails.Frontage.Value.ShouldBe(sourceListing.LandDetails.Frontage.Value);
                destinationListing.LandDetails.IsFrontageModified.ShouldBe(false);
                destinationListing.IsLandDetailsModified.ShouldBe(false);

                for (int i = 0; i < destinationListing.Links.Count; i++)
                {
                    destinationListing.Links[i].ShouldBe(sourceListing.Links[i]);
                }
                destinationListing.IsLinksModified.ShouldBe(false);

                HelperUtilities.AssertMediaItems(destinationListing.Videos, sourceListing.Videos);
                destinationListing.IsImagesModified.ShouldBe(false);
            }

            [Fact]
            public void GivenAnExistingListingAndANewListingWithANullValues_CopyOverNewData_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.ResidentialListingFromFile;
                sourceListing.Agents = null;
                sourceListing.Address = null;
                sourceListing.Features = null;
                sourceListing.FloorPlans = null;
                sourceListing.Images = null;
                sourceListing.Inspections = null;
                sourceListing.LandDetails = null;
                sourceListing.Links = null;
                sourceListing.Videos = null;

                var destinationListing = HelperUtilities.ResidentialListingFromFile;

                // Act.
                destinationListing.CopyOverNewData(sourceListing);

                // Assert.
                destinationListing.Agents.ShouldBe(null);
                destinationListing.IsAgentsModified.ShouldBe(false);
                destinationListing.Address.ShouldBe(null);
                destinationListing.IsAddressModified.ShouldBe(false);
                destinationListing.Features.ShouldBe(null);
                destinationListing.IsFeaturesModified.ShouldBe(false);
                destinationListing.FloorPlans.ShouldBe(null);
                destinationListing.IsFloorPlansModified.ShouldBe(false);
                destinationListing.Images.ShouldBe(null);
                destinationListing.IsImagesModified.ShouldBe(false);
                destinationListing.Inspections.ShouldBe(null);
                destinationListing.IsInspectionsModified.ShouldBe(false);
                destinationListing.LandDetails.ShouldBe(null);
                destinationListing.IsLandDetailsModified.ShouldBe(false);
                destinationListing.Links.ShouldBe(null);
                destinationListing.IsLinksModified.ShouldBe(false);
                destinationListing.Videos.ShouldBe(null);
                destinationListing.IsVideosModified.ShouldBe(false);
            }

            [Fact]
            public void GivenAnExistingListingAndChangingAnImage_CopyOverNewData_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.ResidentialListingFromFile;
                var destinationListing = HelperUtilities.ResidentialListingFromFile;

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
                destinationListing.CopyOverNewData(sourceListing);

                // Assert.
                destinationListing.Images.First().Url.ShouldBe(sourceListing.Images.First().Url);
                destinationListing.IsImagesModified.ShouldBe(false);

                // Now lets change the source and the destination should _not_ change.
                sourceListing.Images.First().Url = "https://1.2.3.4/5";
                destinationListing.Images.First().Url.ShouldNotBe(sourceListing.Images.First().Url);
            }

            [Fact]
            public void GivenAnExistingListingAndChangingAnAgentAfterwards_CopyOverNewData_TheAgentsAreNotBothUpdated()
            {
                // Arrange.
                var sourceListing = HelperUtilities.ResidentialListing;
                var destinationListing = HelperUtilities.ResidentialListingFromFile;

                // Act.
                destinationListing.CopyOverNewData(sourceListing);
                destinationListing.Agents.First().Name = "I'm a Princess";

                // Assert.
                destinationListing.Agents.First().Name.ShouldNotBe(sourceListing.Agents.First().Name);
            }
        }
    }
}