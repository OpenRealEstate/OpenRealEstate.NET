using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core
{
    public abstract class Listing : AggregateRoot
    {
        protected Listing()
        {
            Agents = new List<ListingAgent>();
            Images = new List<Media>();
            FloorPlans = new List<Media>();
            Videos = new List<Media>();
            Inspections = new List<Inspection>();
        }

        public abstract string ListingType { get; }

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

        public IList<string> Links { get; set; }

        public override string ToString()
        {
            return
                $"Agency: {(string.IsNullOrWhiteSpace(AgencyId) ? "--no Agency Id--" : AgencyId)}; Id: {(string.IsNullOrWhiteSpace(Id) ? "--No Id--" : Id)}";
        }
    }
}