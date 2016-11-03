using System;

namespace OpenRealEstate.Core.Models
{
    /* StatusType
     * ^^^^^^^^^^
     * CUR Current
     * WDR Withdrawn
     * SLD Sold
     * LSE Leased
     * OFF Off Market
     */

    public enum StatusType
    {
        Unknown,
        Current,
        Withdrawn,
        Sold,
        Leased,
        OffMarket,
        Deleted
    }

    public static class StatusTypeExtensions
    {
        public static string ToDescription(this StatusType value)
        {
            switch (value)
            {
                case StatusType.Current:
                    return "Current";
                case StatusType.Withdrawn:
                    return "Withdrawn";
                case StatusType.Sold:
                    return "Sold";
                case StatusType.Leased:
                    return "Leased";
                case StatusType.OffMarket: 
                    return "Off Market";
                case StatusType.Deleted:
                    return "Deleted";
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
                case "cur":
                case "current":
                    return StatusType.Current;
                case "wdr":
                case "withdrawn":
                    return StatusType.Withdrawn;
                case "sld":
                case "sold":
                    return StatusType.Sold;
                case "lse":
                case "leased":
                    return StatusType.Leased;
                case "off":
                case "offmarket":
                    return StatusType.OffMarket;
                case "del":
                case "deleted":
                    return StatusType.Deleted;
                default:
                    return StatusType.Unknown;
            }
        }
    }
}