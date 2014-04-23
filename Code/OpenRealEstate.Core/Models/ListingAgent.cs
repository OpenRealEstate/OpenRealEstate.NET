using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class ListingAgent
    {
        public string Name { get; set; }
        public IList<Communication> Communications { get; set; }
        public int Order { get; set; }
    }
}