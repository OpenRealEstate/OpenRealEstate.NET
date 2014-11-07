using System.Collections.Generic;
using System.Linq;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;

namespace OpenRealEstate.WebSite.ViewModels
{
    public class ConvertViewModel
    {
        public int ResidentialCount
        {
            get
            {
                return Listings == null ||
                       !Listings.Any()
                    ? 0
                    : Listings.OfType<ResidentialListing>().Count();
            }
        }

        public int RentalCount
        {
            get
            {
                return Listings == null ||
                       !Listings.Any()
                    ? 0
                    : Listings.OfType<RentalListing>().Count();
            }
        }

        public int RuralCount
        {
            get
            {
                return Listings == null ||
                       !Listings.Any()
                    ? 0
                    : Listings.OfType<RuralListing>().Count();
            }
        }

        public int LandCount
        {
            get
            {
                return Listings == null ||
                       !Listings.Any()
                    ? 0
                    : Listings.OfType<LandListing>().Count();
            }
        }

        public List<Listing> Listings { get; set; }
        public IDictionary<string, string> ValidationErrors { get; set; }
    }
}