using System;

namespace OpenRealEstate.Core
{
    public class Inspection
    {
        public DateTime OpensOn { get; set; }

        public DateTime? ClosesOn { get; set; }

        public override string ToString()
        {
            return $"{OpensOn} <-> {(ClosesOn.HasValue ? ClosesOn.ToString() : "-no time given-")}";
        }
    }
}