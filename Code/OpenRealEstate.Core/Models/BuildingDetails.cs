using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class BuildingDetails
    {
        private UnitOfMeasure _area;
        private decimal? _energyRating;
        private ISet<string> _tags;

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

        public void Copy(BuildingDetails newBuildingDetails)
        {
            if (newBuildingDetails == null)
            {
                throw new ArgumentNullException("newBuildingDetails");
            }

            if (newBuildingDetails.IsAreaModified)
            {
                Area = newBuildingDetails.Area;
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