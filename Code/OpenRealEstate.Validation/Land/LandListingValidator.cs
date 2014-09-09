﻿using System;
using FluentValidation;
using OpenRealEstate.Core.Models.Land;

namespace OpenRealEstate.Validation.Land
{
    public class LandListingValidator : ListingValidator<LandListing>
    {
        public LandListingValidator()
        {
            // Optional.
            RuleFor(listing => listing.AuctionOn).NotEqual(DateTime.MinValue);
            RuleFor(listing => listing.Pricing).SetValidator(new SalePricingValidator());

            // NOTE: No rules needed for listing.LandEstate.
        }
    }
}