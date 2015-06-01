using System;
using Newtonsoft.Json;
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
                return MapExistingListingToNewInstance(newListing);
            }

            var returnListing = MapExistingListingToNewInstance(existingListing);

            // The all important 'new' status for this existing listing :)
            returnListing.StatusType = newListing.StatusType;

            // Assumption: the date the new listing changed status, is the UpdateOn value.
            returnListing.UpdatedOn = newListing.UpdatedOn;

            if ((newListing.StatusType == StatusType.Leased &&
                 newListing is RentalListing &&
                 existingListing is RentalListing) ||
                newListing.StatusType == StatusType.OffMarket ||
                newListing.StatusType == StatusType.Withdrawn)
            {
                // There is no extra data for these statuses.
                return returnListing;
            }

            if (returnListing.StatusType == StatusType.Sold &&
                CanListingBeSold(newListing, returnListing))
            {
                if (newListing is ResidentialListing)
                {
                    var newListingTemp = (ResidentialListing) newListing;
                    var returnListingTemp = (ResidentialListing)returnListing;
                    if (newListingTemp.Pricing != null &&
                        returnListingTemp.Pricing != null)
                    {
                        CopyOverSoldPricingData(newListingTemp.Pricing,
                            returnListingTemp.Pricing);
                    }
                }
                else if (newListing is LandListing)
                {
                    var newListingTemp = (LandListing) newListing;
                    var returnListingTemp = (LandListing)returnListing;
                    if (newListingTemp.Pricing != null &&
                        returnListingTemp.Pricing != null)
                    {
                        CopyOverSoldPricingData(newListingTemp.Pricing,
                            returnListingTemp.Pricing);
                    }
                }
                else if (newListing is RuralListing)
                {
                    var newListingTemp = (RuralListing) newListing;
                    var returnListingTemp = (RuralListing)returnListing;
                    if (newListingTemp.Pricing != null &&
                        returnListingTemp.Pricing != null)
                    {
                        CopyOverSoldPricingData(newListingTemp.Pricing,
                            returnListingTemp.Pricing);
                    }
                }
                else
                {
                    var unhandledErrorMessage = string.Format("Unhandled listing type: '{0}'", newListing.GetType());
                    throw new Exception(unhandledErrorMessage);
                }

                return returnListing;
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

        private static Listing MapExistingListingToNewInstance(Listing existingListing)
        {
            if (existingListing == null)
            {
                throw new Exception();
            }

            var json = JsonConvert.SerializeObject(existingListing);
            if (existingListing is ResidentialListing)
            {
                return JsonConvert.DeserializeObject<ResidentialListing>(json);
            }

            if (existingListing is RentalListing)
            {
                return JsonConvert.DeserializeObject<RentalListing>(json);
            }

            if (existingListing is LandListing)
            {
                return JsonConvert.DeserializeObject<LandListing>(json);
            }

            if (existingListing is RuralListing)
            {
                return JsonConvert.DeserializeObject<RuralListing>(json);
            }

            throw new Exception(string.Format("Unhandled existing listing type => '{0}'.", existingListing.GetType()));
        }
    }
}