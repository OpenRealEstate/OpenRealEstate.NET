using System.Collections.Generic;
using System.Linq;

namespace OpenRealEstate.Core
{
    internal static class HelperUtilities
    {
        internal static void ClearAllObservableCollectionItems<T>(ICollection<T> collection) where T : IModifiedData
        {
            if (collection == null ||
                !collection.Any())
            {
                // Nothing to clear.
                return;
            }

            foreach (var item in collection)
            {
                item.ModifiedData.ClearModifiedPropertiesAndCollections();
            }
        }
    }
}