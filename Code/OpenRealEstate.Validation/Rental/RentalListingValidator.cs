using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using OpenRealEstate.Core.Models.Rental;

namespace OpenRealEstate.Validation.Rental
{
    public class RentalListingValidator : ListingValidator<RentalListing>
    {
        public RentalListingValidator()
        {
            RuleFor(listing => listing.AvailableOn).NotEqual(DateTime.MinValue);
        }
    }
}
