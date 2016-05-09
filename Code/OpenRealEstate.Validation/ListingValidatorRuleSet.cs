using System;
using OpenRealEstate.Core.Residential;

namespace OpenRealEstate.Validation
{
    public enum ListingValidatorRuleSet
    {
        /// <summary>
        /// The bare Minimum validation - harldy <i>any</i> properties are checked.
        /// </summary>
        Minimum,

        /// <summary>
        /// The majority of properties are checked.
        /// </summary>
        Normal,

        /// <summary>
        /// The strictest set of rules which checks most of the properties.
        /// </summary>
        Strict
    }

    public static class ListingValidatorRuleSetExtensions
    {
        public static string ToDescription(this ListingValidatorRuleSet value)
        {
            switch (value)
            {
                case ListingValidatorRuleSet.Minimum:
                    return "default";
                case ListingValidatorRuleSet.Normal:
                    return ListingValidator<ResidentialListing>.NormalRuleSet;
                case ListingValidatorRuleSet.Strict:
                    return ListingValidator<ResidentialListing>.StrictRuleSet;
                default:
                    throw new NotImplementedException(
                        $"A listing validation ruleset enum was not handled. Enum value: '{value}'.");
            }
        }
    }
}