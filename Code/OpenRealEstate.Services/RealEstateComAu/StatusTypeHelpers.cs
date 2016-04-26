using System;
using OpenRealEstate.Core;

namespace OpenRealEstate.Services.RealEstateComAu
{
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
                    return StatusType.Available;
                case "sld":
                case "sold":
                    return StatusType.Sold;
                case "lse":
                case "leased":
                    return StatusType.Leased;
                case "wdr":
                case "withdrawn":
                case "off":
                case "offmarket":
                case "del":
                case "deleted":
                    return StatusType.Removed;
                default:
                    return StatusType.Unknown;
            }
        }
    }
}