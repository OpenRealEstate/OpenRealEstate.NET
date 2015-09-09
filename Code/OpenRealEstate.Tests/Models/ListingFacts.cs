using System.Collections.Generic;
using System.Linq;
using OpenRealEstate.Core.Models;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class ListingFacts
    {
        public class CopyFacts
        {
            [Fact]
            public void GivenAnExistingListingAndANewListingWithEverythingModified_Copy_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing =TestHelperUtilities.ResidentialListingFromFile(false) as Listing;
                
                var destinationListing =TestHelperUtilities.ResidentialListing() as Listing;

                // Act.
                destinationListing.Copy(sourceListing);

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
                destinationListing.Features.Ensuites.ShouldBe(sourceListing.Features.Ensuites);
                destinationListing.Features.IsEnsuitesModified.ShouldBe(true);
                destinationListing.Features.LivingAreas.ShouldBe(sourceListing.Features.LivingAreas);
                destinationListing.Features.IsLivingAreasModified.ShouldBe(true);
                destinationListing.Features.Tags.ShouldBe(sourceListing.Features.Tags);
                destinationListing.Features.IsTagsModified.ShouldBe(true);
                destinationListing.Features.Toilets.ShouldBe(sourceListing.Features.Toilets);
                destinationListing.Features.IsToiletsModified.ShouldBe(true);
                destinationListing.IsFeaturesModified.ShouldBe(true);

                destinationListing.Features.CarParking.Garages.ShouldBe(sourceListing.Features.CarParking.Garages);
                destinationListing.Features.IsCarParkingModified.ShouldBe(true);
                destinationListing.Features.CarParking.Carports.ShouldBe(sourceListing.Features.CarParking.Carports);
                destinationListing.Features.IsCarParkingModified.ShouldBe(true);
                destinationListing.Features.CarParking.OpenSpaces.ShouldBe(sourceListing.Features.CarParking.OpenSpaces);
                destinationListing.Features.CarParking.IsOpenSpacesModified.ShouldBe(true);

               TestHelperUtilities.AssertMedias(destinationListing.FloorPlans, sourceListing.FloorPlans);
                destinationListing.IsFloorPlansModified.ShouldBe(true);

                TestHelperUtilities.AssertMedias(destinationListing.Images, sourceListing.Images);
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

                TestHelperUtilities.AssertMedias(destinationListing.Videos, sourceListing.Videos);
                destinationListing.IsImagesModified.ShouldBe(true);
            }

            [Fact]
            public void GivenAnExistingListingAndANewListingWithANullValues_Copy_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = TestHelperUtilities.ResidentialListingFromFile();

                foreach (var agent in sourceListing.Agents)
                {
                    sourceListing.RemoveAgent(agent);
                }
                sourceListing.Address = null;
                sourceListing.Features = null;

                foreach (var floorPlan in sourceListing.FloorPlans)
                {
                    sourceListing.RemoveFloorPlan(floorPlan);
                }

                foreach (var image in sourceListing.Images)
                {
                    sourceListing.RemoveImage(image);
                }


                foreach (var inspection in sourceListing.Inspections)
                {
                    sourceListing.RemoveInspection(inspection);
                }

                sourceListing.LandDetails = null;
                foreach (var link in sourceListing.Links)
                {
                    sourceListing.RemoveLink(link);
                }

                foreach (var video in sourceListing.Videos)
                {
                    sourceListing.RemoveVideo(video);
                }

                var destinationListing =TestHelperUtilities.ResidentialListingFromFile();

                // Act.
                destinationListing.Copy(sourceListing);

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
            public void GivenAnExistingListingAndChangingAnImage_Copy_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing =TestHelperUtilities.ResidentialListingFromFile();
                var destinationListing =TestHelperUtilities.ResidentialListingFromFile();

                sourceListing.AddImages(new List<Media>
                {
                    new Media
                    {
                        Url = "http://a.b/c",
                        Order = 1,
                        Tag = "hi!"
                    }
                });

                // Act.
                // NOTE: this will just copy over the 1 media item.
                destinationListing.Copy(sourceListing);

                // Assert.
                destinationListing.Images.First().Url.ShouldBe(sourceListing.Images.First().Url);
                destinationListing.IsImagesModified.ShouldBe(true);

                // Now lets change the source and the destination should _not_ change.
                sourceListing.Images.First().Url = "https://1.2.3.4/5";
                destinationListing.Images.First().Url.ShouldNotBe(sourceListing.Images.First().Url);
            }

            [Fact]
            public void GivenAnExistingListingAndChangingAnAgentAfterwards_Copy_TheAgentsAreNotBothUpdated()
            {
                // Arrange.
                var sourceListing =TestHelperUtilities.ResidentialListing(false);
                var destinationListing =TestHelperUtilities.ResidentialListingFromFile();

                // Act.
                destinationListing.Copy(sourceListing);
                destinationListing.Agents.First().Name = "I'm a Princess";

                // Assert.
                destinationListing.Agents.First().Name.ShouldNotBe(sourceListing.Agents.First().Name);
            }
        }

        public class IsAddressIsModifiedFacts
        {
            [Fact]
            public void GivenAListingWithAnExistingAddressAndAStreetUpdated_IsAddressIsModified_ReturnsTrue()
            {
                // Arrange.
                var listing = TestHelperUtilities.ResidentialListingFromFile();
                listing.IsAddressModified.ShouldBe(false);
                const string street = "pewpew";

                // Act.
                listing.Address.Street = street;

                // Arrange.
                listing.Address.Street.ShouldBe(street);
                listing.IsAddressModified.ShouldBe(true);
            }
        }

        public class IsFeaturesIsModifiedFacts
        {
            [Fact]
            public void GivenAListingWithAnExistingFeaturesAndABathroomsUpdated_IsFeaturesIsModified_ReturnsTrue()
            {
                // Arrange.
                var listing = TestHelperUtilities.ResidentialListingFromFile();
                listing.IsFeaturesModified.ShouldBe(false);
                const int bathrooms = 33;

                // Act.
                listing.Features.Bathrooms = bathrooms;

                // Arrange.
                listing.Features.Bathrooms.ShouldBe(bathrooms);
                listing.IsFeaturesModified.ShouldBe(true);
            }
        }

        public class IsLandDetailsIsModifiedFacts
        {
            [Fact]
            public void GivenAListingWithAnExistingFeaturesAndABathroomsUpdated_IsFeaturesIsModified_ReturnsTrue()
            {
                // Arrange.
                var listing = TestHelperUtilities.ResidentialListingFromFile();
                listing.IsLandDetailsModified.ShouldBe(false);
                const string crossOver = "pewpew";

                // Act.
                listing.LandDetails.CrossOver = crossOver;

                // Arrange.
                listing.LandDetails.CrossOver.ShouldBe(crossOver);
                listing.IsLandDetailsModified.ShouldBe(true);
            }
        }

        public class IsModifiedFacts
        {
            [Fact]
            public void GivenAListingAndAnIdUpdated_IsModified_ReturnsTrue()
            {
                // Arrange.
                var listing = TestHelperUtilities.ResidentialListingFromFile();
                listing.IsModified.ShouldBe(false);
                const string id= "1111";

                // Act.
                listing.Id = id;

                // Arrange.
                listing.Id.ShouldBe(id);
                listing.IsModified.ShouldBe(true);
            }
        }
    }
}