using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class LandDetails
    {
        private UnitOfMeasure _area;
        private string _crossOver;
        private IList<Depth> _depths;
        private UnitOfMeasure _frontage;

        public UnitOfMeasure Area
        {
            get { return _area; }
            set
            {
                _area = value;
                IsAreaModified = true;
            }
        }

        public bool IsAreaModified { get; private set; }

        public UnitOfMeasure Frontage
        {
            get { return _frontage; }
            set
            {
                _frontage = value;
                IsFrontageModified = true;
            }
        }

        public bool IsFrontageModified { get; private set; }

        public IList<Depth> Depths
        {
            get { return _depths; }
            set
            {
                _depths = value;
                IsDepthsModified = true;
            }
        }

        public bool IsDepthsModified { get; private set; }

        public string CrossOver
        {
            get { return _crossOver; }
            set
            {
                _crossOver = value;
                IsCrossOverModified = true;
            }
        }

        public bool IsCrossOverModified { get; private set; }

        public void CopyOverNewData(LandDetails newLandDetails)
        {
            if (newLandDetails == null)
            {
                throw new ArgumentNullException("newLandDetails");
            }

            if (newLandDetails.IsAreaModified)
            {
                if (newLandDetails.Area == null)
                {
                    Area = null;
                }
                else
                {
                    if (Area == null)
                    {
                        Area = new UnitOfMeasure();
                    }
                    Area.CopyOverNewData(newLandDetails.Area);
                }
            }

            if (newLandDetails.IsFrontageModified)
            {
                if (newLandDetails.Frontage == null)
                {
                    Frontage = null;
                }
                else
                {
                    if (Frontage == null)
                    {
                        Frontage = new UnitOfMeasure();
                    }
                    Frontage = newLandDetails.Frontage;
                }
            }

            if (newLandDetails.IsDepthsModified)
            {
                Depths = newLandDetails.Depths;
            }

            if (newLandDetails.IsCrossOverModified)
            {
                CrossOver = newLandDetails.CrossOver;
            }
        }

        public void ClearAllIsModified()
        {
            Area.ClearAllIsModified();
            IsAreaModified = false;

            Frontage.ClearAllIsModified();
            IsFrontageModified = false;

            foreach (var depth in Depths)
            {
                depth.ClearAllIsModified();
            }
            IsDepthsModified = false;

            IsCrossOverModified = false;

            
        }
    }
}