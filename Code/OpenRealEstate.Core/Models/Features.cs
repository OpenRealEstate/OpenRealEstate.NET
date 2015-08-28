using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class Features
    {
        private int _bathrooms;
        private int _bedrooms;
        private CarParking _carParking;
        private int _ensuites;
        private bool _isCarParkingModified;
        private int _livingAreas;
        private ISet<string> _tags;
        private int _toilets;

        public int Bedrooms
        {
            get { return _bedrooms; }
            set
            {
                _bedrooms = value;
                IsBedroomsModified = true;
            }
        }

        public bool IsBedroomsModified { get; private set; }

        public int Bathrooms
        {
            get { return _bathrooms; }
            set
            {
                _bathrooms = value;
                IsBathroomsModified = true;
            }
        }

        public bool IsBathroomsModified { get; private set; }

        public int Toilets
        {
            get { return _toilets; }
            set
            {
                _toilets = value;
                IsToiletsModified = true;
            }
        }

        public bool IsToiletsModified { get; private set; }

        public CarParking CarParking
        {
            get { return _carParking; }
            set
            {
                _carParking = value;
                IsCarParkingModified = true;
            }
        }

        public bool IsCarParkingModified
        {
            get
            {
                return _isCarParkingModified ||
                       (CarParking != null &&
                        CarParking.IsModified);
            }
            set { _isCarParkingModified = value; }
        }

        public int Ensuites
        {
            get { return _ensuites; }
            set
            {
                _ensuites = value;
                IsEnsuitesModified = true;
            }
        }

        public bool IsEnsuitesModified { get; private set; }

        public int LivingAreas
        {
            get { return _livingAreas; }
            set
            {
                _livingAreas = value;
                IsLivingAreasModified = true;
            }
        }

        public bool IsLivingAreasModified { get; private set; }

        public ISet<string> Tags
        {
            get { return _tags; }
            set
            {
                _tags = value;
                IsTagsModified = true;
            }
        }

        public bool IsTagsModified { get; private set; }

        public bool IsModified
        {
            get
            {
                return IsBedroomsModified ||
                       IsBathroomsModified ||
                       IsToiletsModified ||
                       IsCarParkingModified ||
                       IsEnsuitesModified ||
                       IsLivingAreasModified ||
                       IsTagsModified;
            }
        }

        public void Copy(Features newFeatures)
        {
            if (newFeatures == null)
            {
                throw new ArgumentNullException("newFeatures");
            }

            if (newFeatures.IsBedroomsModified)
            {
                Bedrooms = newFeatures.Bedrooms;
            }

            if (newFeatures.IsBathroomsModified)
            {
                Bathrooms = newFeatures.Bathrooms;
            }

            if (newFeatures.IsToiletsModified)
            {
                Toilets = newFeatures.Toilets;
            }

            if (newFeatures.IsCarParkingModified)
            {
                if (newFeatures.CarParking == null)
                {
                    CarParking = null;
                }
                else
                {
                    if (CarParking == null)
                    {
                        CarParking = new CarParking();
                    }

                    if (newFeatures.CarParking.IsModified)
                    {
                        CarParking.Copy(newFeatures.CarParking);
                    }

                    IsCarParkingModified = true;
                }
            }

            if (newFeatures.IsEnsuitesModified)
            {
                Ensuites = newFeatures.Ensuites;
            }

            if (newFeatures.IsLivingAreasModified)
            {
                LivingAreas = newFeatures.LivingAreas;
            }

            if (newFeatures.IsTagsModified)
            {
                Tags = newFeatures.Tags;
            }
        }

        public void ClearAllIsModified()
        {
            IsBedroomsModified = false;
            IsBathroomsModified = false;
            IsToiletsModified = false;
            IsCarParkingModified = false;
            IsEnsuitesModified = false;
            IsLivingAreasModified = false;
            IsTagsModified = false;

            if (CarParking != null)
            {
                CarParking.ClearAllIsModified();
            }
        }
    }
}