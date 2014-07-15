using System;

namespace OpenRealEstate.Core.Models
{
    public abstract class AggregateRoot
    {
        public string Id { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}