using System;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class BuildingDetails
    {
        private readonly DecimalNullableNotified _energyRating;
        private readonly ModifiedData _modifiedData;
        private UnitOfMeasure _area;
        private bool _isAreaModified;

        public BuildingDetails()
        {
            _modifiedData = new ModifiedData(GetType());
            _energyRating = new DecimalNullableNotified("EnergyRating");
            _energyRating.PropertyChanged += _modifiedData.OnPropertyChanged;
        }

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
            get { return _energyRating.Value; }
            set { _energyRating.Value = value; }
        }

        [Obsolete]
        public bool IsEnergyRatingModified { get; private set; }

        public bool IsModified
        {
            get { return _modifiedData.IsModified; }
        }

        public void Copy(BuildingDetails newBuildingDetails)
        {
            _modifiedData.Copy(newBuildingDetails, this);


            //if (newBuildingDetails == null)
            //{
            //    throw new ArgumentNullException("newBuildingDetails");
            //}

            //if (newBuildingDetails.IsAreaModified)
            //{
            //    if (newBuildingDetails.Area == null)
            //    {
            //        Area = null;
            //    }
            //    else
            //    {
            //        if (Area== null)
            //        {
            //            Area = new UnitOfMeasure();
            //        }

            //        if (newBuildingDetails.Area.IsModified)
            //        {
            //            Area = newBuildingDetails.Area;
            //        }

            //        IsAreaModified = true;
            //    }
            //}

            //if (newBuildingDetails.IsEnergyRatingModified)
            //{
            //    EnergyRating = newBuildingDetails.EnergyRating;
            //}
        }

        public void ClearAllIsModified()
        {
            _modifiedData.ClearModifiedProperties(new[] {"EnergyRating"});
            //if (Area != null)
            //{
            //    Area.ClearAllIsModified();
            //}
            //IsAreaModified = false;

            //IsEnergyRatingModified = false;
        }
    }
}