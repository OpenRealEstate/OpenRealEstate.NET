using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenRealEstate.Core.Models
{
    public class Agent : AggregateRoot
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public IList<string> AgencyIds { get; set; }
        public string Description { get; set; }
        public IList<Communication> Communications { get; set; }
        public Dictionary<string, string> WebSites { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }

        /// <summary>
        /// The ability to order profiles by the importance within the Agency. 
        /// </summary>
        /// <remarks>eg. Principals may wish to be first in the list of agents for an agency.
        /// </remarks>
        public string Order { get; set; }

        public IList<Media> Images { get; set; }

        public void Validate(Dictionary<string, string> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }


            if (string.IsNullOrWhiteSpace(Name))
            {
                errors.Add("Name", "A name is required. eg. Jane Smith.");
            }

            if (AgencyIds == null ||
                !AgencyIds.Any())
            {
                errors.Add("AgencyIds", "At least one AgencyId is requires where this Agent works at.");
            }
        }
    }
}