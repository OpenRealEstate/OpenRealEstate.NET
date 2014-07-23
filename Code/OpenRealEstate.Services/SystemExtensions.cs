using System;
using Shouldly;

namespace OpenRealEstate.Services
{
    public static class SystemExtensions
    {
        public static bool ParseOneYesZeroNoToBool(this string value)
        {
            value.ShouldNotBeNullOrEmpty();

            switch (value.ToUpperInvariant())
            {
                case "1":
                case "Y":
                case "YES":
                    return true;
                case "0":
                case "N":
                case "NO":
                    return false;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }
    }
}