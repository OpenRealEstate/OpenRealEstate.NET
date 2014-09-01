﻿using System;

namespace OpenRealEstate.Core.Models.Land
{
    public class LandListing : Listing
    {
        public CategoryType CategoryType { get; set; }
        public SalePricing Pricing { get; set; }
        public DateTime? AuctionOn { get; set; }
        public LandEstate Estate { get; set; }

        public override string ToString()
        {
            return string.Format("Land >> {0}", base.ToString());
        }
    }
}