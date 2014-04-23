using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public abstract class Listing : AggregateRoot
    {
        public string AgencyId { get; set; }
        public StatusType StatusType { get; set; }
        public PropertyType PropertyType { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public IList<ListingAgent> Agents { get; set; }
        public Features Features { get; set; }
        public IList<Media> Images { get; set; }
        public IList<Media> FloorPlans { get; set; }
        public IList<Media> Videos { get; set; }
        public IList<Inspection> Inspections { get; set; }

        public new void Validate(Dictionary<string, string> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }

            base.Validate(errors);

            if (StatusType == StatusType.Unknown)
            {
                errors.Add("StatusType", "Invalid StatusType. Please choose any status except Unknown.");
            }

            Address.Validate(errors);
        }
    }
}