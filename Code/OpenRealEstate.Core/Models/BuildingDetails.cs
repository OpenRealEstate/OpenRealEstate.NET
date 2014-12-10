using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class BuildingDetails
    {
        public UnitOfMeasure Area { get; set; }
        public decimal? EnergyRating { get; set; }
        public ISet<string> Tags { get; set; }
    }
}