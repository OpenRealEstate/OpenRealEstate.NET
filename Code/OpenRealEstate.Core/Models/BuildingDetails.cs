using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class BuildingDetails : BaseModifiedData
    {
        private const string AreaName = "Area";
        private const string EnergyRatingName = "EnergyRating";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<UnitOfMeasure> _area;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DecimalNullableNotified _energyRating;

        public BuildingDetails()
        {
            _energyRating = new DecimalNullableNotified(EnergyRatingName);
            _energyRating.PropertyChanged += ModifiedData.OnPropertyChanged;

            _area = new InstanceObjectNotified<UnitOfMeasure>(AreaName);
            _area.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

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

        public void Copy(BuildingDetails newBuildingDetails, bool isModifiedPropertiesOnly = true)
        {
            ModifiedData.Copy(newBuildingDetails, this, isModifiedPropertiesOnly);
        }

        public void ClearAllIsModified()
        {
            if (_area.Value != null &&
                _area.Value.ModifiedData.IsModified)
            {
                _area.Value.ClearAllIsModified();
            }

            ModifiedData.ClearModifiedPropertiesAndCollections();
        }
    }
}