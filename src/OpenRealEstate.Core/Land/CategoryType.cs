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

            if (value.Equals("COMMERICAL", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Commercial;
            }

            if (value.Equals("RESIDENTIAL", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Residential;
            }

            return CategoryType.Unknown;
        }
    }
}