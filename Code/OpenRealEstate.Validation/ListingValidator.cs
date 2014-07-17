using System;
using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class ListingValidator<T> : AggregateRootValidator<T> where T : Listing
    {
        public const string MinimumRuleSet = "default," + MinimumRuleSetKey;
        protected const string MinimumRuleSetKey = "Minimum";

        public ListingValidator()
        {
            // Required.
            RuleFor(listing => listing.AgencyId).NotEmpty()
                .WithMessage("Every listing needs at least one 'AgencyId'. eg. FancyPants-1234a or 456123, etc.");
            RuleFor(listing => listing.StatusType).NotEqual(StatusType.Unknown)
                .WithMessage("Invalid 'StatusType'. Please choose any status except Unknown.");
            RuleFor(listing => listing.CreatedOn).NotEqual(DateTime.MinValue)
                .WithMessage(
                    "A valid 'UpdatedOn' is required. Please use a date/time value that is in this decade or so.");
            
            // Minimum data required to have a listing.
            RuleSet(MinimumRuleSetKey, () =>
            {
                // Required.
                RuleFor(listing => listing.Title).NotEmpty();
                RuleFor(listing => listing.Description).NotEmpty();
                RuleFor(listing => listing.Address).NotNull().SetValidator(new AddressValidator());
            
                // Optional.
                RuleFor(listing => listing.Agents).SetCollectionValidator(new ListingAgentValidator());
                RuleFor(listing => listing.Images).SetCollectionValidator(new MediaValidator());
                RuleFor(listing => listing.FloorPlans).SetCollectionValidator(new MediaValidator());
                RuleFor(listing => listing.Videos).SetCollectionValidator(new MediaValidator());
                RuleFor(listing => listing.Inspections).SetCollectionValidator(new InspectionValidator());
                RuleFor(listing => listing.LandDetails).SetValidator(new LandDetailsValidator());
                RuleFor(listing => listing.Features).SetValidator(new FeaturesValidator());
            });
        }
    }
}