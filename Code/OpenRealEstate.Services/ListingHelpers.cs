using System;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;

namespace OpenRealEstate.Services
{
    public static class ListingHelpers
    {
        public static Listing CopyOverListingData(Listing newListing, Listing existingListing)
        {
            if (existingListing == null)
            {
                throw new ArgumentNullException("existingListing");
            }

            if (newListing == null)
            {
                throw new ArgumentNullException("newListing");
            }

            if (newListing.StatusType == StatusType.Current)
            {
                // We don't need anything from the existing listing - so turf it.
                return newListing;
            }

            // The all important 'new' status for this existing listing :)
            existingListing.StatusType = newListing.StatusType;

            // Assumption: the date the new listing changed status, is the UpdateOn value.
            existingListing.UpdatedOn = newListing.UpdatedOn;

            if ((newListing.StatusType == StatusType.Leased &&
                 newListing is RentalListing &&
                 existingListing is RentalListing) ||
                newListing.StatusType == StatusType.OffMarket ||
                newListing.StatusType == StatusType.Withdrawn)
            {
                // There is no extra data for these statuses.
                return existingListing;
            }

            if (existingListing.StatusType == StatusType.Sold &&
                CanListingBeSold(newListing, existingListing))
            {
                if (newListing is ResidentialListing)
                {
                    CopyOverSoldPricingData(((ResidentialListing) newListing).Pricing,
                        ((ResidentialListing) existingListing).Pricing);
                }
                else if (newListing is LandListing)
                {
                    CopyOverSoldPricingData(((LandListing)newListing).Pricing,
                        ((LandListing)existingListing).Pricing);
                }
                else if (newListing is RuralListing)
                {
                    CopyOverSoldPricingData(((RuralListing)newListing).Pricing,
                        ((RuralListing)existingListing).Pricing);
                }
                else
                {
                    var unhandledErrorMessage = string.Format("Unhandled listing type: '{0}'", newListing.GetType());
                    throw new Exception(unhandledErrorMessage);
                }

                return existingListing;
            }

            var errorMessage =
                string.Format(
                    "Unhandled status type -or- incompatible status type with the listing type (ie. a residential listing cannot be leased ... only rental listings can be leased. New listing: '{0}' - '{1}'. Existing listing: '{2}' - '{3}'",
                    newListing,
                    newListing.StatusType,
                    existingListing,
                    existingListing.StatusType);
            throw new Exception(errorMessage);
        }

        private static void CopyOverSoldPricingData(SalePricing newSalePricing, SalePricing existingSalePricing)
        {
            if (newSalePricing == null)
            {
                throw new ArgumentNullException("newSalePricing");
            }

            if (existingSalePricing == null)
            {
                throw new ArgumentNullException("existingSalePricing");
            }

            existingSalePricing.SoldPrice = newSalePricing.SoldPrice;
            existingSalePricing.SoldPriceText = newSalePricing.SoldPriceText;
            existingSalePricing.SoldOn = newSalePricing.SoldOn;
        }

        private static bool CanListingBeSold(Listing newListing, Listing existingListing)
        {
            return ((newListing is ResidentialListing &&
                     existingListing is ResidentialListing) ||
                    (newListing is LandListing &&
                     existingListing is LandListing) ||
                    (newListing is RuralListing &&
                     existingListing is RuralListing));
        }
    }
}