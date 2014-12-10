using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class Features
    {
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Toilets { get; set; }
        public int Ensuites { get; set; }
        public int Garages { get; set; }
        public int Carports { get; set; }
        public int LivingAreas { get; set; }
        public int OpenSpaces { get; set; }
        public ISet<string> Tags { get; set; }
    }
}