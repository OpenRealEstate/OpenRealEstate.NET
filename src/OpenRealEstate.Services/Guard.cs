using System;


namespace OpenRealEstate.Services
{
    internal static class Guard
    {
        internal static void AgainstNull(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }

        internal static void AgainstNullOrWhiteSpace(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
        }
    }
}
