using System;

namespace OpenRealEstate.Core.Models
{
    public class Inspection
    {
        private DateTime? _closesOn;
        private DateTime _opensOn;

        public DateTime OpensOn
        {
            get { return _opensOn; }
            set
            {
                _opensOn = value;
                IsOpensOnModified = true;
            }
        }

        public bool IsOpensOnModified { get; private set; }

        public DateTime? ClosesOn
        {
            get { return _closesOn; }
            set
            {
                _closesOn = value;
                IsClosesOnModified = true;
            }
        }

        public bool IsClosesOnModified { get; private set; }

        public void CopyOverNewData(Inspection inspection)
        {
            if (inspection == null)
            {
                throw new ArgumentNullException("inspection");
            }

            if (inspection.IsClosesOnModified)
            {
                ClosesOn = inspection.ClosesOn;
            }

            if (inspection.IsOpensOnModified)
            {
                OpensOn = inspection.OpensOn;
            }
        }

        public void ClearAllIsModified()
        {
            IsOpensOnModified = false;
            IsClosesOnModified = false;
        }
    }
}