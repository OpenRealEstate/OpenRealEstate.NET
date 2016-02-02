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

        internal static void AgainstNullOrWhiteSpace(string stringObj)
        {
            if (string.IsNullOrWhiteSpace(stringObj))
            {
                throw new ArgumentNullException(nameof(stringObj));
            }
        }
    }
}
