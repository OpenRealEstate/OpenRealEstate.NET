using System;
using Shouldly;

namespace OpenRealEstate.Services
{
    public static class SystemExtensions
    {
        public static bool ParseYesNoToBool(this string value)
        {
            value.ShouldNotBeNullOrEmpty();

            switch (value.ToUpperInvariant())
            {
                case "Y":
                case "YES":
                    return true;
                case "N":
                case "NO":
                    return false;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }
    }
}