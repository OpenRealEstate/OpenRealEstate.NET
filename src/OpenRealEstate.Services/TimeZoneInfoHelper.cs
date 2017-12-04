using System;
using System.Collections.Generic;

namespace OpenRealEstate.Services
{
    public static class TimeZoneInfoHelper
    {
        // REF: https://msdn.microsoft.com/en-us/library/ms912391(v=winembedded.11).aspx
        private static readonly Dictionary<string, string> StateTimeZoneInfos =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {"vic", "AUS Eastern Standard Time"},
                {"victoria", "AUS Eastern Standard Time"},
                {"nsw", "AUS Eastern Standard Time"},
                {"new south wales", "AUS Eastern Standard Time"},
                {"act", "AUS Eastern Standard Time"},
                {"australian capital territory", "AUS Eastern Standard Time"},
                {"tas", "Tasmania Standard Time"},
                {"tasmania", "Tasmania Standard Time"},
                {"qld", "E. Australia Standard Time"},
                {"queensland", "E. Australia Standard Time"},
                {"nt", "AUS Central Standard Time"},
                {"northern territory", "AUS Central Standard Time"},
                {"sa", "Cen. Australia Standard Time"},
                {"south australia", "Cen. Australia Standard Time"},
                {"wa", "W. Australia Standard Time"},
                {"western australia", "W. Australia Standard Time"}
            };

        public static TimeZoneInfo ConvertByState(string state)
        {
            Guard.AgainstNullOrWhiteSpace(state);

            if (StateTimeZoneInfos.ContainsKey(state))
            {
                // Do we have a match?
                return TimeZoneInfo.FindSystemTimeZoneById(StateTimeZoneInfos[state]);
            }

            throw new ArgumentException($"State value '{state}' provided is not a valid Australian state.");
        }
    }
}