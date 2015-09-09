using System;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class BuildingDetails
    {
        private readonly DecimalNullableNotified _energyRating;
        private readonly InstanceObjectNotified<UnitOfMeasure> _area;
        [Obsolete]
        private bool _isAreaModified;
        private const string AreaName = "Area";
        private const string EnergyRatingName = "EnergyRating";

        public BuildingDetails()
        {
            ModifiedData = new ModifiedData(GetType());

            _energyRating = new DecimalNullableNotified(EnergyRatingName);
            _energyRating.PropertyChanged += ModifiedData.OnPropertyChanged;

            _area = new InstanceObjectNotified<UnitOfMeasure>(AreaName);
            _area.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }

        public UnitOfMeasure Area
        {
            get { return _area.Value; }
            set { _area.Value = value; }
        }
        [Obsolete]
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
            get { return ModifiedData.IsModified; }
        }

        public void Copy(BuildingDetails newBuildingDetails)
        {
            ModifiedData.Copy(newBuildingDetails, this);
        }

        public void ClearAllIsModified()
        {
            if (_area != null &&
                _area.Value.IsModified)
            {
                _area.Value.ClearAllIsModified();
            }

            ModifiedData.ClearModifiedProperties(new[] { EnergyRatingName, AreaName });
        }
    }
}