using System;

namespace OpenRealEstate.Core.Land
{
    public enum CategoryType
    {
        Unknown,
        Residential,
        Commercial
    }

    public static class CategoryTypeHelpers
    {
        public static CategoryType ToCategoryType(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            switch (value.ToUpperInvariant())
            {
                case "COMMERICAL":
                    return CategoryType.Commercial;
                case "RESIDENTIAL":
                    return CategoryType.Residential;
                default:
                    return CategoryType.Unknown;
            }
        }
    }
}