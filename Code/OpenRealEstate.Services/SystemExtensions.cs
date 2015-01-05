using System;
using System.Collections.Generic;
using Shouldly;

namespace OpenRealEstate.Services
{
    public static class SystemExtensions
    {
        private static readonly ISet<string> yesSet = 
            new HashSet<string>(new []{"y", "yes"}, StringComparer.InvariantCultureIgnoreCase);
        private static readonly ISet<string> noSet = 
            new HashSet<string>(new [] {"n", "no"}, StringComparer.InvariantCultureIgnoreCase);

        public static bool ParseYesOrNoToBool(this string value)
        {
            value.ShouldNotBeNullOrEmpty();

            if (yesSet.Contains(value))
            {
                return true;
            }

            if (noSet.Contains(value))
            {
                return false;
            }

            throw new ArgumentOutOfRangeException("value");
        }

        public static bool TryParseYesOrNoToBool(this string value, out bool result)
        {
            value.ShouldNotBeNullOrEmpty();

            if (yesSet.Contains(value))
            {
                result = true;
                return true;
            }

            result = false;
            return noSet.Contains(value);
        }
       
        public static bool ParseOneYesZeroNoToBool(this string value)
        {
            value.ShouldNotBeNullOrEmpty();

            if (value == "1" ||
                yesSet.Contains(value))
            {
                return true;
            }

            if (value == "0" ||
                noSet.Contains(value))
            {
                return false;
            }

            throw new ArgumentOutOfRangeException("value",
                string.Format("Value '{0}' is out of range. It should only be 0/1/yes/no.",
                    string.IsNullOrWhiteSpace(value)
                        ? "-null-"
                        : value));
        }

        public static byte ParseByteValueOrDefault(this string value)
        {
            byte number = 0;
            if (string.IsNullOrEmpty(value))
            {
                return number;
            }

            if (byte.TryParse(value, out number))
            {
                return number;
            }

            var errorMessage = string.Format("Failed to parse the value '{0}' into a byte.", value);
            throw new Exception(errorMessage);
        }
    }
}