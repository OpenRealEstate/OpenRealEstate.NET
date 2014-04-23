using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class Agency : AggregateRoot
    {
        public string FranchiseId { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public IList<Communication> Communications { get; set; }
        public Dictionary<string, string> WebSites { get; set; }
        public IList<Media> Logos { get; set; }
        public IList<Media> Images { get; set; }
        public IList<Media> Videos { get; set; }

        public new void Validate(Dictionary<string, string> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }
            base.Validate(errors);

            if (string.IsNullOrWhiteSpace(FranchiseId))
            {
                errors.Add("FranchiseId", "A FranchiseId is required. An Agency has to be part of a franchise (including sole agencies).");
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                errors.Add("Name", "A name is required.");
            }
        }
    }
}