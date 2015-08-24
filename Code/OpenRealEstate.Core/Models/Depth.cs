using System;

namespace OpenRealEstate.Core.Models
{
    public class Depth : UnitOfMeasure
    {
        private string _side;

        public string Side
        {
            get { return _side; }
            set
            {
                _side = value;
                IsSideModified = true;
            }
        }

        public bool IsSideModified { get; private set; }

        public void Copy(Depth newDepth)
        {
            if (newDepth == null)
            {
                throw new ArgumentNullException("newDepth");
            }

            if (newDepth.IsSideModified)
            {
                Side = newDepth.Side;
            }

            base.Copy(newDepth);
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            IsSideModified = false;
        }
    }
}