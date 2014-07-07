using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models.Residential
{
    public class ResidentialListing : Listing
    {
        public PropertyType PropertyType { get; set; }
        public SalePricing Pricing { get; set; }
        public DateTime? AuctionOn { get; set; }

        public new void Validate(Dictionary<string, string> errors, string keySuffix = null)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }

            // Each key needs to be determined by the Agency + Unique id. This way,
            // when we are validating multiple listings, we don't get a duplicate key-conflict.
            if (string.IsNullOrWhiteSpace(keySuffix))
            {
                keySuffix = string.Empty;
            }

            base.Validate(errors, keySuffix);

            if (PropertyType == PropertyType.Unknown)
            {
                errors.Add("PropertyType" + keySuffix, "Invalid PropertyType. Please choose any property except Unknown.");
            }
        }
    }
}