using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class BuildingDetails
    {
        private const string AreaName = "Area";
        private const string EnergyRatingName = "EnergyRating";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<UnitOfMeasure> _area;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DecimalNullableNotified _energyRating;

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

        public decimal? EnergyRating
        {
            get { return _energyRating.Value; }
            set { _energyRating.Value = value; }
        }

        public void Copy(BuildingDetails newBuildingDetails)
        {
            ModifiedData.Copy(newBuildingDetails, this);
        }

        public void ClearAllIsModified()
        {
            if (_area != null &&
                _area.Value.ModifiedData.IsModified)
            {
                _area.Value.ClearAllIsModified();
            }

            ModifiedData.ClearModifiedProperties(new[] {EnergyRatingName, AreaName});
        }
    }
}