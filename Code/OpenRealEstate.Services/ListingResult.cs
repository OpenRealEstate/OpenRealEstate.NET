using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Services
{
    public class ListingResult
    {
        public Listing Listing { get; set; }
        public string SourceData { get; set; }

        public override string ToString()
        {
            return string.Format("Type: {0}",
                Listing == null
                    ? "-null -"
                    : Listing.GetType().ToString());
        }
    }
}