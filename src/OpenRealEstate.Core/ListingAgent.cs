using System.Collections.Generic;
using System.Linq;

namespace OpenRealEstate.Core
{
    public class ListingAgent
    {
        public string Name { get; set; }

        public IList<Communication> Communications { get; set; }

        public int Order { get; set; }

        public override string ToString()
        {
            return
                $"{Order} {(string.IsNullOrWhiteSpace(Name) ? "-no name-" : Name)} - C: {(Communications != null && Communications.Any() ? Communications.Count : 0)}";
        }
    }
}