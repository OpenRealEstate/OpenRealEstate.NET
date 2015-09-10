using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        internal static void SetCollection<T>(ObservableCollection<T> source, ICollection<T> value, Action<ICollection<T>> action) where T : class
        {
            if ((source == null &&
                value == null) ||
                (source != null &&
                !source.Any() &&
                value != null &&
                !value.Any()))
            {
                // Nothing to set ... and we don't want the collection events to fire off.
                return;
            }

            if (source != null)
            {
                source.Clear();
            }

            if (value != null &&
                value.Any())
            {
                action(value);
            }
        }

    }
}