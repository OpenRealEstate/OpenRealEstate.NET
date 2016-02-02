using OpenRealEstate.Core;
using System;
using System.Collections.Generic;

namespace OpenRealEstate.Services
{
    public static class SystemExtensions
    {
        private static readonly ISet<string> YesTrueSet = 
            new HashSet<string>(new []{"y", "yes", "true"}, StringComparer.InvariantCultureIgnoreCase);
        private static readonly ISet<string> NoFalseSet = 
            new HashSet<string>(new [] {"n", "no", "false"}, StringComparer.InvariantCultureIgnoreCase);

        public static bool ParseYesTrueOrNoFalseToBool(this string value)
        {
            Guard.AgainstNullOrWhiteSpace(value);

            if (YesTrueSet.Contains(value))
            {
                return true;
            }

            if (NoFalseSet.Contains(value))
            {
                return false;
            }

            throw new ArgumentOutOfRangeException("value");
        }

        public static bool TryParseYesOrNoToBool(this string value, out bool result)
        {
            Guard.AgainstNullOrWhiteSpace(value);

            if (YesTrueSet.Contains(value))
            {
                result = true;
                return true;
            }

            result = false;
            return NoFalseSet.Contains(value);
        }
       
        public static bool ParseOneYesZeroNoToBool(this string value)
        {
            Guard.AgainstNullOrWhiteSpace(value);

            if (value == "1" ||
                YesTrueSet.Contains(value))
            {
                return true;
            }

            if (value == "0" ||
                NoFalseSet.Contains(value))
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

            var tempNumber = ParseNumberToIntOrDefault(value);
            if (byte.TryParse(tempNumber.ToString(), out number))
            {
                return number;
            }

            var errorMessage = string.Format("Failed to parse the value '{0}' into a byte.", value);
            throw new Exception(errorMessage);
        }

        private static int ParseNumberToIntOrDefault(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            // NOTE: We first convert to a float. This is because 
            //    a) a decimal value with ZERO's for the decimal place == ok.
            //    b) a decimal value with numbers for the decimal place == bad.
            float number;

            // REF: http://stackoverflow.com/questions/6598179/the-right-way-to-compare-a-system-double-to-0-a-number-int
            //      
            if (float.TryParse(value, out number) &&
                Math.Abs((number % 1)) < 0.01)
            {
                return (int)number;
            }

            var errorMessage = string.Format("Failed to parse the value '{0}' into an int. Is it a valid number? Does it contain decimal point values?", value);
            throw new Exception(errorMessage);
        }
    }
}