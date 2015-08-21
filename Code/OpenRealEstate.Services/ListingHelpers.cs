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
        public static void Copy(ResidentialListing existingListing,
            ResidentialListing updatedListing)
        {
            CopyListings(existingListing, updatedListing);    
        }

        public static void Copy(RentalListing existingListing,
            RentalListing updatedListing)
        {
            CopyListings(existingListing, updatedListing);
        }

        public static void Copy(RuralListing existingListing,
            RuralListing updatedListing)
        {
            CopyListings(existingListing, updatedListing);
        }

        public static void Copy(LandListing existingListing,
            LandListing updatedListing)
        {
            CopyListings(existingListing, updatedListing);
        }

        private static void CopyListings(Listing existingListing,
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
            var updatedResidentialListing = updatedListing as ResidentialListing;
            if (residentialListing != null &&
                updatedResidentialListing != null)
            {
                residentialListing.Copy(updatedResidentialListing);
                return;
            }

            var rentalListing = existingListing as RentalListing;
            var updatedRentalListing = updatedListing as RentalListing;
            if (rentalListing != null &&
                updatedRentalListing != null)
            {
                rentalListing.Copy(updatedRentalListing);
                return;
            }

            var ruralListing = existingListing as RuralListing;
            var updatedRuralListig = updatedListing as RuralListing;
            if (ruralListing != null &&
                updatedRuralListig != null)
            {
                ruralListing.Copy(updatedRuralListig);
                return;
            }

            var landListing = existingListing as LandListing;
            var updatedLandListing = updatedListing as LandListing;
            if (landListing != null &&
                updatedLandListing != null)
            {
                landListing.Copy(updatedLandListing);
                return;
            }

            var errorMessage =
                string.Format(
                    "Both listings have to be of the same type when trying to Copy data from one listing to another. Desintation listing [{0}] ; Source listing [{1}].",
                    existingListing.GetType().FullName,
                    updatedListing.GetType().FullName);
            throw new Exception(errorMessage);
        }
    }
}