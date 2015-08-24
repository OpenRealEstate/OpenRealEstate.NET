using System;
using System.Linq;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Core.Filters
{
    public static class AggregateRootFilters
    {
        public static IQueryable<AggregateRoot> WithId(this IQueryable<AggregateRoot> value, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }

            return value.Where(x => x.Id == id);
        }
    }
}