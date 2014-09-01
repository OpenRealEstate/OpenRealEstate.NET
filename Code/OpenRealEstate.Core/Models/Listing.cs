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
        public LandDetails LandDetails { get; set; }
        public Features Features { get; set; }

        public override string ToString()
        {
            return string.Format("Agency: {0}; Id: {1}",
                string.IsNullOrWhiteSpace(AgencyId)
                    ? "--no Agency Id--"
                    : AgencyId,
                string.IsNullOrWhiteSpace(Id)
                    ? "--No Id--"
                    : Id);
        }
    }
}