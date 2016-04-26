using System.Collections.Generic;

namespace OpenRealEstate.Core
{
    public class LandDetails
    {
        public LandDetails()
        {
            Depths = new List<Depth>();
        }

        public UnitOfMeasure Area { get; set; }

        public UnitOfMeasure Frontage { get; set; }

        public IList<Depth> Depths { get; set; }

        public string CrossOver { get; set; }
    }
}