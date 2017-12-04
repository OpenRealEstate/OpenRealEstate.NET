using System;
using System.Linq;
using FluentValidation;
using OpenRealEstate.Core;

namespace OpenRealEstate.Validation
{
    public class ListingValidator<T> : AggregateRootValidator<T> where T : Listing
    {
        public const string NormalRuleSet = "default," + NormalRuleSetKey;
        public const string StrictRuleSet = NormalRuleSet + "," + StrictRuleSetKey;
        protected const string NormalRuleSetKey = "Normal";
        protected const string StrictRuleSetKey = "Strict";

        public ListingValidator()
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            // Minimum required data required to have a listing.
            RuleFor(listing => listing.AgencyId)
                .NotEmpty()
                .WithMessage("Every listing needs at least one 'AgencyId'. eg. FancyPants-1234a or 456123, etc.");
            RuleFor(listing => listing.StatusType)
                .NotEqual(StatusType.Unknown)
                .WithMessage("Invalid 'StatusType'. Please choose any status except Unknown.");
            RuleFor(listing => listing.CreatedOn)
                .NotEqual(DateTime.MinValue)
                .WithMessage(
                    "A valid 'CreatedOn' is required. Please use a date/time value that is in this decade or so.");

            // Normal rules to check, when we have a property to check.
            RuleSet(NormalRuleSetKey,
                    () =>
                    {
                        // Required.
                        RuleFor(listing => listing.Title).NotEmpty();
                        RuleFor(listing => listing.Address).NotNull().SetValidator(new AddressValidator());

                        // Required where it exists.
                        RuleFor(listing => listing.Agents).SetCollectionValidator(new ListingAgentValidator());
                        RuleFor(listing => listing.Images).SetCollectionValidator(new MediaValidator());
                        RuleFor(listing => listing.FloorPlans).SetCollectionValidator(new MediaValidator());
                        RuleFor(listing => listing.Videos).SetCollectionValidator(new MediaValidator());
                        RuleFor(listing => listing.Inspections).SetCollectionValidator(new InspectionValidator());
                        RuleFor(listing => listing.LandDetails).SetValidator(new LandDetailsValidator());
                        RuleFor(listing => listing.Features).SetValidator(new FeaturesValidator());
                    });

            // Strictest of rules to check existing properties.
            RuleSet(StrictRuleSetKey,
                    () =>
                    {
                        // Required where it exists.
                        RuleForEach(listing => listing.Links)
                            .Must(LinkMustBeAUri)
                            .When(listing => listing.Links != null &&
                                             listing.Links.Any())
                            .WithMessage("Link '{PropertyValue}' must be a valid URI. eg: http://www.SomeWebSite.com.au");
                    });
        }

        private static bool LinkMustBeAUri(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return false;
            }

            return Uri.TryCreate(link, UriKind.Absolute, out var result) &&
                   (result.Scheme == Uri.UriSchemeHttp ||
                    result.Scheme == Uri.UriSchemeHttps);
        }
    }
}