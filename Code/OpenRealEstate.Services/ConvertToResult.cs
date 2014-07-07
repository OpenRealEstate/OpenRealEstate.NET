using System.Collections.Generic;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Services
{
    public class ConvertToResult
    {
        public IList<Listing> Listings { get; set; }
        public IList<string> UnhandledData { get; set; }
    }
}