using System.Collections.Generic;

namespace OpenRealEstate.Core
{
    public class Features
    {
        public Features()
        {
            Tags = new HashSet<string>();
        }

        public byte Bedrooms { get; set; }

        public byte Bathrooms { get; set; }

        public byte Toilets { get; set; }

        public CarParking CarParking { get; set; }

        public byte Ensuites { get; set; }

        public byte LivingAreas { get; set; }

        public HashSet<string> Tags { get; set; }
    }
}