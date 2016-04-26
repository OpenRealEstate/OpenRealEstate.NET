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

            switch (value.ToLowerInvariant())
            {
                case "av":
                case "avl":
                case "avail":
                case "available":
                    return StatusType.Available;
                case "sld":
                case "sold":
                    return StatusType.Sold;
                case "lse":
                case "leased":
                    return StatusType.Leased;
                case "rem":
                case "remove":
                case "removed":
                    return StatusType.Removed;
                default:
                    return StatusType.Unknown;
            }
        }
    }
}