using System;


namespace OpenRealEstate.Core
{
    public static class Guard
    {
        public static void AgainstNull(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }

        public static void AgainstNullOrWhiteSpace(string stringObj)
        {
            if (string.IsNullOrWhiteSpace(stringObj))
            {
                throw new ArgumentNullException(nameof(stringObj));
            }
        }
    }
}
