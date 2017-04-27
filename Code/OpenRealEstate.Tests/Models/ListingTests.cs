using System.Collections.Generic;
using System.Linq;
using OpenRealEstate.Core.Models;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class ListingTests
    {
        public class CopyTests
        {
            [Fact]
            public void GivenAnExistingListingAndANewListingWithEverythingModified_Copy_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = TestHelperUtilities.ResidentialListingFromFile(false) as Listing;
                var destinationListing = TestHelperUtilities.ResidentialListing() as Listing;

                // Act.
                destinationListing.Copy(sourceListing);

                // Assert.
                destinationListing.AgencyId.ShouldBe(sourceListing.AgencyId);
                destinationListing.CreatedOn.ShouldBe(sourceListing.CreatedOn);
                destinationListing.Description.ShouldBe(sourceListing.Description);
                destinationListing.Id.ShouldBe(sourceListing.Id);
                destinationListing.StatusType.ShouldBe(sourceListing.StatusType);
                destinationListing.Title.ShouldBe(sourceListing.Title);

                destinationListing.Address.CountryIsoCode.ShouldBe(sourceListing.Address.CountryIsoCode);
                destinationListing.Address.IsStreetDisplayed.ShouldBe(sourceListing.Address.IsStreetDisplayed);
                destinationListing.Address.Latitude.ShouldBe(sourceListing.Address.Latitude);
                destinationListing.Address.Longitude.ShouldBe(sourceListing.Address.Longitude);
                destinationListing.Address.Municipality.ShouldBe(sourceListing.Address.Municipality);
                destinationListing.Address.Postcode.ShouldBe(sourceListing.Address.Postcode);
                destinationListing.Address.State.ShouldBe(sourceListing.Address.State);
                destinationListing.Address.Street.ShouldBe(sourceListing.Address.Street);
                destinationListing.Address.StreetNumber.ShouldBe(sourceListing.Address.StreetNumber);
                destinationListing.Address.Suburb.ShouldBe(sourceListing.Address.Suburb);

                for (int i = 0; i < destinationListing.Agents.Count; i++)
                {
                    destinationListing.Agents[i].Name.ShouldBe(sourceListing.Agents[i].Name);
                    destinationListing.Agents[i].Order.ShouldBe(sourceListing.Agents[i].Order);
                    for (int j = 0; j < destinationListing.Agents[i].Communications.Count; j++)
                    {
                        destinationListing.Agents[i].Communications[j].CommunicationType.ShouldBe(
                            sourceListing.Agents[i].Communications[j].CommunicationType);
                        destinationListing.Agents[i].Communications[j].Details.ShouldBe(
                            sourceListing.Agents[i].Communications[j].Details);
                    }
                }

                destinationListing.Features.Bathrooms.ShouldBe(sourceListing.Features.Bathrooms);
                destinationListing.Features.Bedrooms.ShouldBe(sourceListing.Features.Bedrooms);
                destinationListing.Features.Ensuites.ShouldBe(sourceListing.Features.Ensuites);
                destinationListing.Features.LivingAreas.ShouldBe(sourceListing.Features.LivingAreas);
                destinationListing.Features.Tags.ShouldBe(sourceListing.Features.Tags);
                destinationListing.Features.Toilets.ShouldBe(sourceListing.Features.Toilets);

                destinationListing.Features.CarParking.Garages.ShouldBe(sourceListing.Features.CarParking.Garages);
                destinationListing.Features.CarParking.Carports.ShouldBe(sourceListing.Features.CarParking.Carports);
                destinationListing.Features.CarParking.OpenSpaces.ShouldBe(sourceListing.Features.CarParking.OpenSpaces);

                TestHelperUtilities.AssertMedias(destinationListing.FloorPlans, sourceListing.FloorPlans);
                TestHelperUtilities.AssertMedias(destinationListing.Images, sourceListing.Images);

                for (var i = 0; i < destinationListing.Inspections.Count; i++)
                {
                    destinationListing.Inspections[0].OpensOn.ShouldBe(sourceListing.Inspections[0].OpensOn);
                    destinationListing.Inspections[0].ClosesOn.ShouldBe(sourceListing.Inspections[0].ClosesOn);
                }

                destinationListing.LandDetails.Area.Type.ShouldBe(sourceListing.LandDetails.Area.Type);
                destinationListing.LandDetails.Area.Value.ShouldBe(sourceListing.LandDetails.Area.Value);
                destinationListing.LandDetails.CrossOver.ShouldBe(sourceListing.LandDetails.CrossOver);
                for (int i = 0; i < destinationListing.LandDetails.Depths.Count; i++)
                {
                    destinationListing.LandDetails.Depths[i].Type.ShouldBe(sourceListing.LandDetails.Depths[i].Type);
                    destinationListing.LandDetails.Depths[i].Value.ShouldBe(sourceListing.LandDetails.Depths[i].Value);
                    destinationListing.LandDetails.Depths[i].Side.ShouldBe(sourceListing.LandDetails.Depths[i].Side);
                }
                destinationListing.LandDetails.Frontage.Type.ShouldBe(sourceListing.LandDetails.Frontage.Type);
                destinationListing.LandDetails.Frontage.Value.ShouldBe(sourceListing.LandDetails.Frontage.Value);

                for (var i = 0; i < destinationListing.Links.Count; i++)
                {
                    destinationListing.Links[i].ShouldBe(sourceListing.Links[i]);
                }

                TestHelperUtilities.AssertMedias(destinationListing.Videos, sourceListing.Videos);
                TestHelperUtilities.AssertMedias(destinationListing.Documents, sourceListing.Documents);
            }

            [Fact]
            public void GivenAnExistingListingAndANewListingWithSomeNullValues_Copy_CopiesOverTheData()
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

                foreach (var document in sourceListing.Documents)
                {
                    sourceListing.RemoveDocuments(document);
                }

                var destinationListing = TestHelperUtilities.ResidentialListingFromFile();

                // Act.
                destinationListing.Copy(sourceListing);

                // Assert.
                destinationListing.Agents.Count.ShouldBe(0);
                destinationListing.Address.ShouldBeNull();
                destinationListing.Features.ShouldBeNull();
                destinationListing.FloorPlans.Count.ShouldBe(0);
                destinationListing.Images.Count.ShouldBe(0);
                destinationListing.Inspections.Count.ShouldBe(0);
                destinationListing.LandDetails.ShouldBeNull();
                destinationListing.Links.Count.ShouldBe(0);
                destinationListing.Videos.Count.ShouldBe(0);
                destinationListing.Documents.Count.ShouldBe(0);
            }

            [Fact]
            public void GivenAnExistingListingAndChangingAnImage_Copy_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = TestHelperUtilities.ResidentialListingFromFile();
                var destinationListing = TestHelperUtilities.ResidentialListingFromFile();

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

                // Now lets change the source and the destination should _not_ change.
                sourceListing.Images.First().Url = "https://1.2.3.4/5";
                destinationListing.Images.First().Url.ShouldNotBe(sourceListing.Images.First().Url);
            }

            [Fact]
            public void GivenAnExistingListingAndChangingAnAgentAfterwards_Copy_TheAgentsAreNotBothUpdated()
            {
                // Arrange.
                var sourceListing = TestHelperUtilities.ResidentialListing(false);
                var destinationListing = TestHelperUtilities.ResidentialListingFromFile();

                // Act.
                destinationListing.Copy(sourceListing);
                destinationListing.Agents.First().Name = "I'm a Princess";

                // Assert.
                destinationListing.Agents.First().Name.ShouldNotBe(sourceListing.Agents.First().Name);
            }
        }
    }
}