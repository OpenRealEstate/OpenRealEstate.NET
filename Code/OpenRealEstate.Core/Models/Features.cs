using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class Features
    {
        private int _bathrooms;
        private int _bedrooms;
        private int _carports;
        private int _ensuites;
        private int _garages;
        private int _livingAreas;
        private int _openSpaces;
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

        public int Garages
        {
            get { return _garages; }
            set
            {
                _garages = value;
                IsGaragesModified = true;
            }
        }

        public bool IsGaragesModified { get; private set; }

        public int Carports
        {
            get { return _carports; }
            set
            {
                _carports = value;
                IsCarportsModified = true;
            }
        }

        public bool IsCarportsModified { get; private set; }

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
        
        public int OpenSpaces
        {
            get { return _openSpaces; }
            set
            {
                _openSpaces = value;
                IsOpenSpacesModified = true;
            }
        }

        public bool IsOpenSpacesModified { get; private set; }

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

            if (newFeatures.IsEnsuitesModified)
            {
                Ensuites = newFeatures.Ensuites;
            }

            if (newFeatures.IsGaragesModified)
            {
                Garages = newFeatures.Garages;
            }

            if (newFeatures.IsCarportsModified)
            {
                Carports = newFeatures.Carports;
            }

            if (newFeatures.IsLivingAreasModified)
            {
                LivingAreas = newFeatures.LivingAreas;
            }

            if (newFeatures.IsOpenSpacesModified)
            {
                OpenSpaces = newFeatures.OpenSpaces;
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
            IsEnsuitesModified = false;
            IsGaragesModified = false;
            IsCarportsModified = false;
            IsLivingAreasModified = false;
            IsOpenSpacesModified = false;
            IsTagsModified = false;
        }
    }
}