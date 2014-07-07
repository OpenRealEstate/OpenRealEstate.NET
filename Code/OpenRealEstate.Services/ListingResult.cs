using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Services
{
    public class ListingResult
    {
        public Listing Listing { get; set; }
        public string SourceData { get; set; }
    }
}