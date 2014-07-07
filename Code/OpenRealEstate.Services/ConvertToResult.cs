using System.Collections.Generic;

namespace OpenRealEstate.Services
{
    public class ConvertToResult
    {
        public IList<ListingResult> Listings { get; set; }
        public IList<string> UnhandledData { get; set; }
    }
}