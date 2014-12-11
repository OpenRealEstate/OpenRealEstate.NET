using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class LandDetails
    {
        public UnitOfMeasure Area { get; set; }
        public UnitOfMeasure Frontage { get; set; }
        public IList<Depth> Depths { get; set; }
        public string CrossOver { get; set; }
    }
}