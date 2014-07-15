using System;
using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class ListingValidator<T> : AggregateRootValidator<T> where T : Listing
    {
        public ListingValidator()
        {
            RuleFor(listing => listing.AgencyId).NotEmpty()
                .WithMessage("Every listing needs at least one AgencyId. eg. FancyPants-1234a or 456123, etc.");
            RuleFor(listing => listing.StatusType).NotEqual(StatusType.Unknown)
                .WithMessage("Invalid StatusType. Please choose any status except Unknown.");
            RuleFor(listing => listing.CreatedOn).NotEqual(DateTime.MinValue)
                .WithMessage(
                    "The Date/Time value is illegal. Please use a valid value, which is a more current value .. like .. something from this century, please, or a NULL value (ie. Not sure when it closes on).");

            RuleSet("Current", () =>
            {
                RuleFor(listing => listing.Title).NotEmpty();
                RuleFor(listing => listing.Description).NotEmpty();
                RuleFor(listing => listing.Address).SetValidator(new AddressValidator());
                RuleFor(listing => listing.Agents).SetCollectionValidator(new ListingAgentValidator());
                //RuleFor(listing => listing.Images).SetCollectionValidator(new AgentValidator());
                //RuleFor(listing => listing.FloorPlans).SetCollectionValidator(new AgentValidator());
                //RuleFor(listing => listing.Videos).SetCollectionValidator(new AgentValidator());
                RuleFor(listing => listing.Inspections).SetCollectionValidator(new InspectionValidator());
                RuleFor(listing => listing.LandDetails).SetValidator(new LandDetailsValidator());
                RuleFor(listing => listing.Features).SetValidator(new FeaturesValidator());
            });
        }
    }
}