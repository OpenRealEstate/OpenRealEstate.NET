using System;

namespace OpenRealEstate.Core.Rural
{
    public enum CategoryType
    {
        Unknown,
        Cropping,
        Dairy,
        Farmlet,
        Horticulture,
        Lifestyle,
        Livestock,
        MixedFarming,
        Viticulture,
        Other
    }

    public static class CategoryTypeExtensions
    {
        public static string ToDescription(this CategoryType value)
        {
            switch (value)
            {
                case CategoryType.Cropping:
                    return "Cropping";
                case CategoryType.Dairy:
                    return "Dairy";
                case CategoryType.Farmlet:
                    return "Farmlet";
                case CategoryType.Horticulture:
                    return "Horticulture";
                case CategoryType.Lifestyle:
                    return "Lifestyle";
                case CategoryType.Livestock:
                    return "Livestock";
                case CategoryType.MixedFarming:
                    return "Mixed Farming";
                case CategoryType.Viticulture:
                    return "Viticulture";
                case CategoryType.Other:
                    return "Other";
                default:
                    return "Unknown";
            }
        }
    }

    public static class CategoryTypeHelpers
    {
        public static CategoryType ToCategoryType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException();
            }

            if (value.Equals("CROPPING", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Cropping;
            }

            if (value.Equals("DAIRY", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Dairy;
            }

            if (value.Equals("FARMLET", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Farmlet;
            }

            if (value.Equals("HORTICULTURE", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Horticulture;
            }

            if (value.Equals("LIFESTYLE", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Lifestyle;
            }

            if (value.Equals("LIVESTOCK", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Livestock;
            }

            if (value.Equals("MIXEDFARMING", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("MIXED FARMING", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.MixedFarming;
            }

            if (value.Equals("VITICULTURE", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Viticulture;
            }

            if (value.Equals("OTHER", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Other;
            }

            return CategoryType.Unknown;
        }
    }
}