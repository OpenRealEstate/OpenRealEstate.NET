using System;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;

namespace OpenRealEstate.Services
{
    public class ListingHelpers
    {
        private static void Copy(Listing existingListing,
            Listing updatedListing)
        {
            if (existingListing == null)
            {
                throw new ArgumentNullException("existingListing");
            }

            if (updatedListing == null)
            {
                throw new ArgumentNullException("updatedListing");
            }

            var residentialListing = existingListing as ResidentialListing;
            if (residentialListing != null)
            {
                residentialListing.Copy(updatedListing);
                return;
            }

            var rentalListing = existingListing as RentalListing;
            if (rentalListing != null)
            {
                rentalListing.Copy(updatedListing);
                return;
            }

            var ruralListing = existingListing as RuralListing;
            if (ruralListing != null)
            {
                ruralListing.Copy(updatedListing);
                return;
            }

            var landListing = existingListing as LandListing;
            if (landListing != null)
            {
                landListing.Copy(updatedListing);
                return;
            }

            var errorMessage =
                string.Format(
                    "Failed to determine the type of listing [{0}] when trying to Copy data from one listing to another.",
                    existingListing.GetType().FullName);
            throw new Exception(errorMessage);
        }
    }
}