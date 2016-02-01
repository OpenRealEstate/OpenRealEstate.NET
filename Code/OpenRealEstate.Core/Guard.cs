using System;


namespace OpenRealEstate.Core
{
    public class Guard
    {
        public static void AgainstNulls(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }

        public static void AgainstNulls(string stringObj)
        {
            if (string.IsNullOrEmpty(stringObj))
            {
                throw new ArgumentNullException(nameof(stringObj));
            }
        }
    }
}
