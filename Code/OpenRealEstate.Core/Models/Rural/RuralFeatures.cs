using System;

namespace OpenRealEstate.Core.Models.Rural
{
    public class RuralFeatures
    {
        private string _annualRainfall;
        private string _carryingCapacity;
        private string _fencing;
        private string _improvements;
        private string _irrigation;
        private string _services;
        private string _soilTypes;

        public string Fencing
        {
            get { return _fencing; }
            set
            {
                _fencing = value;
                IsFencingModified = true;
            }
        }

        public bool IsFencingModified { get; set; }

        public string AnnualRainfall
        {
            get { return _annualRainfall; }
            set
            {
                _annualRainfall = value;
                IsAnnualRainfallModified = true;
            }
        }

        public bool IsAnnualRainfallModified { get; set; }

        public string SoilTypes
        {
            get { return _soilTypes; }
            set
            {
                _soilTypes = value;
                IsSoilTypesModified = true;
            }
        }

        public bool IsSoilTypesModified { get; set; }

        public string Improvements
        {
            get { return _improvements; }
            set
            {
                _improvements = value;
                IsImprovementsModified = true;
            }
        }

        public bool IsImprovementsModified { get; set; }

        public string Irrigation
        {
            get { return _irrigation; }
            set
            {
                _irrigation = value;
                IsIrrigationModified = true;
            }
        }

        public bool IsIrrigationModified { get; set; }

        public string CarryingCapacity
        {
            get { return _carryingCapacity; }
            set
            {
                _carryingCapacity = value;
                IsCarryingCapacityModified = true;
            }
        }

        public bool IsCarryingCapacityModified { get; set; }

        public string Services
        {
            get { return _services; }
            set
            {
                _services = value;
                IsServicesModified = true;
            }
        }

        public bool IsServicesModified { get; set; }

        public void CopyOverNewData(RuralFeatures newRuralFeatures)
        {
            if (newRuralFeatures == null)
            {
                throw new ArgumentNullException("newRuralFeatures");
            }

            if (newRuralFeatures.IsFencingModified)
            {
                Fencing = newRuralFeatures.Fencing;
            }

            if (newRuralFeatures.IsAnnualRainfallModified)
            {
                AnnualRainfall = newRuralFeatures.AnnualRainfall;
            }

            if (newRuralFeatures.IsSoilTypesModified)
            {
                SoilTypes = newRuralFeatures.SoilTypes;
            }

            if (newRuralFeatures.IsImprovementsModified)
            {
                Improvements = newRuralFeatures.Improvements;
            }

            if (newRuralFeatures.IsIrrigationModified)
            {
                Irrigation = newRuralFeatures.Irrigation;
            }

            if (newRuralFeatures.IsCarryingCapacityModified)
            {
                CarryingCapacity = newRuralFeatures.CarryingCapacity;
            }

            if (newRuralFeatures.IsServicesModified)
            {
                Services = newRuralFeatures.Services;
            }
        }

        public void ClearAllIsModified()
        {
            IsFencingModified = false;
            IsAnnualRainfallModified = false;
            IsSoilTypesModified = false;
            IsImprovementsModified = false;
            IsIrrigationModified = false;
            IsCarryingCapacityModified = false;
            IsServicesModified = false;
        }
    }
}