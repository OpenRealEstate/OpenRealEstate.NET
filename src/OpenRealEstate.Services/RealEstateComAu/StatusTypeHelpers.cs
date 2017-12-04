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

            if (value.Equals("cur", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("current", StringComparison.OrdinalIgnoreCase))
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

            if (value.Equals("wdr", StringComparison.OrdinalIgnoreCase) ||
                     value.Equals("withdrawn", StringComparison.OrdinalIgnoreCase) ||
                     value.Equals("off", StringComparison.OrdinalIgnoreCase) ||
                     value.Equals("offmarket", StringComparison.OrdinalIgnoreCase) ||
                     value.Equals("del", StringComparison.OrdinalIgnoreCase) ||
                     value.Equals("deleted", StringComparison.OrdinalIgnoreCase))
            {
                return StatusType.Removed;
            }

            return StatusType.Unknown;
        }
    }
}