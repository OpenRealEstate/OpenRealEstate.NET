using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenRealEstate.Core.Models
{
    public abstract class Listing : AggregateRoot
    {
        public string AgencyId { get; set; }
        public StatusType StatusType { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public IList<ListingAgent> Agents { get; set; }
        public IList<Media> Images { get; set; }
        public IList<Media> FloorPlans { get; set; }
        public IList<Media> Videos { get; set; }
        public IList<Inspection> Inspections { get; set; }

        public new void Validate(Dictionary<string, string> errors, string keySuffix = null)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }

            // Each key needs to be determined by the Agency + Unique id. This way,
            // when we are validating multiple listings, we don't get a duplicate key-conflict.
            if (string.IsNullOrWhiteSpace(keySuffix))
            {
                keySuffix = string.Empty;
            }

            base.Validate(errors, keySuffix);

            if (StatusType == StatusType.Unknown)
            {
                errors.Add("StatusType" + keySuffix, "Invalid StatusType. Please choose any status except Unknown.");
            }

            if (StatusType == StatusType.Current)
            {
                ValidateACurrentStatusType(errors, keySuffix);
            }
        }

        private void ValidateACurrentStatusType(Dictionary<string, string> errors, string keySuffix = null)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                errors.Add("Title" + keySuffix, "A title is required.");
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                errors.Add("Description" + keySuffix, "A description is required.");
            }

            if (Address == null)
            {
                errors.Add("Address" + keySuffix, "Address information is required.");
            }
            else
            {
                Address.Validate(errors, keySuffix);
            }

            if (Agents != null &&
                Agents.Any())
            {
                foreach (var agent in Agents)
                {
                    agent.Validate(errors, keySuffix);
                }
            }

            if (Images != null &&
                Images.Any())
            {
                foreach (var image in Images)
                {
                    image.Validate(errors, keySuffix);
                }
            }

            if (FloorPlans != null &&
                FloorPlans.Any())
            {
                foreach (var floorPlan in FloorPlans)
                {
                    floorPlan.Validate(errors, keySuffix);
                }
            }

            if (Videos != null &&
                Videos.Any())
            {
                foreach (var video in Videos)
                {
                    video.Validate(errors, keySuffix);
                }
            }

            if (Inspections != null &&
                Inspections.Any())
            {
                foreach (var inspection in Inspections)
                {
                    inspection.Validate(errors, keySuffix);
                }
            }
        }
    }
}