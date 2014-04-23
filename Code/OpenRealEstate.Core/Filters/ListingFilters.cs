using System;
using System.Linq;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Core.Filters
{
    public static class ListingFilters
    {
        public static IQueryable<Listing> WithId(this IQueryable<Listing> value, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }

            return value.Where(x => x.Id == id);
        }
    }
}