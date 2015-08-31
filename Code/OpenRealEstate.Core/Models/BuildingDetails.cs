using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class BuildingDetails
    {
        private UnitOfMeasure _area;
        private bool _isAreaModified;
        private decimal? _energyRating;

        public UnitOfMeasure Area
        {
            get { return _area; }
            set
            {
                _area = value;
                IsAreaModified = true;
            }
        }

        public bool IsAreaModified
        {
            get
            {
                return _isAreaModified ||
                       (Area != null &&
                        Area.IsModified);
            }
            set { _isAreaModified = value; }
        }

        public decimal? EnergyRating
        {
            get { return _energyRating; }
            set
            {
                _energyRating = value;
                IsEnergyRatingModified = true;
            }
        }

        public bool IsEnergyRatingModified { get; private set; }

        public bool IsModified
        {
            get
            {
                return IsAreaModified ||
                       IsEnergyRatingModified;
            }
        }

        public void Copy(BuildingDetails newBuildingDetails)
        {
            if (newBuildingDetails == null)
            {
                throw new ArgumentNullException("newBuildingDetails");
            }

            if (newBuildingDetails.IsAreaModified)
            {
                if (newBuildingDetails.Area == null)
                {
                    Area = null;
                }
                else
                {
                    if (Area== null)
                    {
                        Area = new UnitOfMeasure();
                    }

                    if (newBuildingDetails.Area.IsModified)
                    {
                        Area = newBuildingDetails.Area;
                    }

                    IsAreaModified = true;
                }
            }

            if (newBuildingDetails.IsEnergyRatingModified)
            {
                EnergyRating = newBuildingDetails.EnergyRating;
            }
        }

        public void ClearAllIsModified()
        {
            if (Area != null)
            {
                Area.ClearAllIsModified();
            }
            IsAreaModified = false;

            IsEnergyRatingModified = false;
        }
    }
}