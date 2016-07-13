using System;
using System.Collections.Generic;
using System.Linq;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Rental;
using Shouldly;

namespace OpenRealEstate.Tests.Transmorgrifiers.REA
{
    public static class ListingAssertHelpers
    {
        public static void AssertCommonData(Listing source, Listing destination)
        {
            source.ShouldNotBeNull();
            destination.ShouldNotBeNull();

            source.AgencyId.ShouldBe(destination.AgencyId);
            source.Id.ShouldBe(destination.Id);
            source.CreatedOn.ShouldBe(destination.CreatedOn);
            source.StatusType.ShouldBe(destination.StatusType);
            source.Description.ShouldBe(destination.Description);
            source.Title.ShouldBe(destination.Title);

            AssertAddress(source.Address, destination.Address);
            AssertAgents(source.Agents, destination.Agents);
            AssertFeatures(source.Features, destination.Features);
            AssertMedia(source.FloorPlans, destination.FloorPlans);
            AssertMedia(source.Images, destination.Images);
            AssertInspections(source.Inspections, destination.Inspections);
            AssertLandDetails(source.LandDetails, destination.LandDetails);
            AssertLinks(source.Links, destination.Links);
            AssertMedia(source.Videos, destination.Videos);
        }

        public static void AssertAddress(Address source, Address destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            source.ShouldNotBeNull();
            destination.ShouldNotBeNull();

            source.IsStreetDisplayed.ShouldBe(destination.IsStreetDisplayed);
            source.StreetNumber.ShouldBe(destination.StreetNumber);
            source.Street.ShouldBe(destination.Street);
            source.Suburb.ShouldBe(destination.Suburb);
            source.Municipality.ShouldBe(destination.Municipality);
            source.State.ShouldBe(destination.State);
            source.CountryIsoCode.ShouldBe(destination.CountryIsoCode);
            source.Postcode.ShouldBe(destination.Postcode);
            source.Latitude.ShouldBe(destination.Latitude);
            source.Longitude.ShouldBe(destination.Longitude);
        }

        public static void AssertAgents(IList<ListingAgent> source, IList<ListingAgent> destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            destination.Count.ShouldBe(destination.Count);

            for (var i = 0; i < destination.Count; i++)
            {
                source[i].Name.ShouldBe(destination[i].Name);
                source[i].Order.ShouldBe(destination[i].Order);

                source[i].Communications.Count.ShouldBe(destination[i].Communications.Count);

                for (var j = 0; j < destination[i].Communications.Count; j++)
                {
                    AssertCommunication(source[i].Communications[j], destination[i].Communications[j]);
                }
            }
        }

        public static void AssertCommunication(Communication source, Communication destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            source.CommunicationType.ShouldBe(destination.CommunicationType);
            source.Details.ShouldBe(destination.Details);
        }

        public static void AssertInspections(IList<Inspection> source, IList<Inspection> destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            for (var i = 0; i < destination.Count; i++)
            {
                source[i].OpensOn.ShouldBe(destination[i].OpensOn);
                source[i].ClosesOn.ShouldBe(destination[i].ClosesOn);
            }
        }

        public static void AssertFeatures(Features source, Features destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            source.Bedrooms.ShouldBe(destination.Bedrooms);
            source.Bathrooms.ShouldBe(destination.Bathrooms);
            source.Ensuites.ShouldBe(destination.Ensuites);
            source.Toilets.ShouldBe(destination.Toilets);
            source.LivingAreas.ShouldBe(destination.LivingAreas);

            AssertTags(source.Tags, destination.Tags);
            AssertCarParking(source.CarParking, destination.CarParking);
        }

        public static void AssertCarParking(CarParking source, CarParking destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            source.Garages.ShouldBe(destination.Garages);
            source.Carports.ShouldBe(destination.Carports);
            source.OpenSpaces.ShouldBe(destination.OpenSpaces);
        }

        public static void AssertTags(ICollection<string> source, IEnumerable<string> destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            var missingTags = destination.Except(source, StringComparer.OrdinalIgnoreCase).ToList();
            if (missingTags.Any())
            {
                var errorMessage =
                    $"Failed to parse - the following tags haven't been handled: {string.Join(", ", missingTags)}.";
                throw new Exception(errorMessage);
            }
        }

        public static void AssertMedia(IList<Media> source, IList<Media> destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            for (var i = 0; i < destination.Count; i++)
            {
                source[i].Url.ShouldBe(destination[i].Url);
                source[i].Order.ShouldBe(destination[i].Order);
                source[i].CreatedOn.ShouldBe(destination[i].CreatedOn);
                source[i].Tag.ShouldBe(destination[i].Tag);
            }
        }

        public static void AssertLandDetails(LandDetails source, LandDetails destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            UnitOfMeasureAssertHelpers.AssertUnitOfMeasure(source.Area, destination.Area);
            UnitOfMeasureAssertHelpers.AssertUnitOfMeasure(source.Frontage, destination.Frontage);
            source.CrossOver.ShouldBe(destination.CrossOver);

            for (var i = 0; i < destination.Depths.Count; i++)
            {
                AssertDepth(source.Depths[i], destination.Depths[i]);
            }
        }

        public static void AssertDepth(Depth source, Depth destination)
        {
            if (source == null &&
               destination == null)
            {
                return;
            }

            source.Side.ShouldBe(destination.Side);
            UnitOfMeasureAssertHelpers.AssertUnitOfMeasure(source, destination);
        }

        public static void AssertLinks(IList<string> source, IList<string> destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            destination.Count.ShouldBe(destination.Count);

            for (var i = 0; i < destination.Count(); i++)
            {
                source[i].ShouldBe(destination[i]);
            }
        }
    }
}