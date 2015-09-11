using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models.Rural
{
    public class RuralFeatures : BaseModifiedData
    {
        private const string AnnualRainfallName = "AnnualRainfall";
        private const string CarryingCapacityName = "CarryingCapacity";
        private const string FencingName = "Fencing";
        private const string ImprovementsName = "Improvements";
        private const string IrrigationName = "Irrigation";
        private const string ServicesName = "Services";
        private const string SoilTypesName = "SoilTypes";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _annualRainfall;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _carryingCapacity;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _fencing;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _improvements;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _irrigation;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _services;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _soilTypes;

        public RuralFeatures()
        {
            _annualRainfall = new StringNotified(AnnualRainfallName);
            _annualRainfall.PropertyChanged += ModifiedData.OnPropertyChanged;

            _carryingCapacity = new StringNotified(CarryingCapacityName);
            _carryingCapacity.PropertyChanged += ModifiedData.OnPropertyChanged;

            _fencing = new StringNotified(FencingName);
            _fencing.PropertyChanged += ModifiedData.OnPropertyChanged;

            _improvements = new StringNotified(ImprovementsName);
            _improvements.PropertyChanged += ModifiedData.OnPropertyChanged;

            _irrigation = new StringNotified(IrrigationName);
            _irrigation.PropertyChanged += ModifiedData.OnPropertyChanged;

            _services = new StringNotified(ServicesName);
            _services.PropertyChanged += ModifiedData.OnPropertyChanged;

            _soilTypes = new StringNotified(SoilTypesName);
            _soilTypes.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public string Fencing
        {
            get { return _fencing.Value; }
            set { _fencing.Value = value; }
        }

        public string AnnualRainfall
        {
            get { return _annualRainfall.Value; }
            set { _annualRainfall.Value = value; }
        }

        public string SoilTypes
        {
            get { return _soilTypes.Value; }
            set { _soilTypes.Value = value; }
        }

        public string Improvements
        {
            get { return _improvements.Value; }
            set { _improvements.Value = value; }
        }

        public string Irrigation
        {
            get { return _irrigation.Value; }
            set { _irrigation.Value = value; }
        }

        public string CarryingCapacity
        {
            get { return _carryingCapacity.Value; }
            set { _carryingCapacity.Value = value; }
        }

        public string Services
        {
            get { return _services.Value; }
            set { _services.Value = value; }
        }

        public void Copy(RuralFeatures newRuralFeatures, bool isModifiedPropertiesOnly = true)
        {
            if (newRuralFeatures == null)
            {
                throw new ArgumentNullException("newRuralFeatures");
            }

            ModifiedData.Copy(newRuralFeatures, this, isModifiedPropertiesOnly);
        }
    }
}