using System;

namespace OpenRealEstate.Core.Models.Rural
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

            switch (value.ToUpperInvariant())
            {
                case "CROPPING":
                    return CategoryType.Cropping;
                case "DAIRY":
                    return CategoryType.Dairy;
                case "FARMLET":
                    return CategoryType.Farmlet;
                case "HORTICULTURE":
                    return CategoryType.Horticulture;
                case "LIFESTYLE":
                    return CategoryType.Lifestyle;
                case "LIVESTOCK":
                    return CategoryType.Livestock;
                case "MIXEDFARMING":
                case "MIXED FARMING":
                    return CategoryType.MixedFarming;
                case "VITICULTURE":
                    return CategoryType.Viticulture;
                case "OTHER":
                    return CategoryType.Other;
                default:
                    return CategoryType.Unknown;
            }
        }
    }
}