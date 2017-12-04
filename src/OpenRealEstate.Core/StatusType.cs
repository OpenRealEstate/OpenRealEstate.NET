using System;

namespace OpenRealEstate.Core
{
    public enum StatusType
    {
        Unknown,
        Available,
        Sold,
        Leased,
        Removed
    }

    public static class StatusTypeExtensions
    {
        public static string ToDescription(this StatusType value)
        {
            switch (value)
            {
                case StatusType.Available:
                    return "Available";
                case StatusType.Sold:
                    return "Sold";
                case StatusType.Leased:
                    return "Leased";
                case StatusType.Removed:
                    return "Removed";
                default:
                    return "Unknown";
            }
        }
    }

    public static class StatusTypeHelpers
    {
        public static StatusType ToStatusType(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Equals("av", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("avl", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("avail", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("available", StringComparison.OrdinalIgnoreCase))
            {
                return StatusType.Available;
            }

            if (value.Equals("sld", StringComparison.OrdinalIgnoreCase) || 
                value.Equals("sold", StringComparison.OrdinalIgnoreCase))
            {
                return StatusType.Sold;
            }

            if (value.Equals("lse", StringComparison.OrdinalIgnoreCase) || 
                value.Equals("leased", StringComparison.OrdinalIgnoreCase))
            {
                return StatusType.Leased;
            }

            if (value.Equals("rem", StringComparison.OrdinalIgnoreCase) || 
                value.Equals("remove", StringComparison.OrdinalIgnoreCase) || 
                value.Equals("removed", StringComparison.OrdinalIgnoreCase))
            {
                return StatusType.Removed;
            }

            return StatusType.Unknown;
        }
    }
}