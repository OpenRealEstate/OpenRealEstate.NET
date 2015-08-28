using System;

namespace OpenRealEstate.Core.Models
{
    public class CarParking
    {
        private int _carports;
        private int _garages;
        private int _openspaces;

        public int Garages
        {
            get { return _garages; }
            set
            {
                _garages = value;
                IsGaragesModified = true;
            }
        }

        public bool IsGaragesModified { get; set; }

        public int Carports
        {
            get { return _carports; }
            set
            {
                _carports = value;
                IsCarportsModified = true;
            }
        }

        public bool IsCarportsModified { get; set; }

        public int OpenSpaces
        {
            get { return _openspaces; }
            set
            {
                _openspaces = value;
                IsOpenSpacesModified = true;
            }
        }

        public bool IsOpenSpacesModified { get; set; }

        public int TotalCount
        {
            get { return Garages + Carports + OpenSpaces; }
        }

        public bool IsModified
        {
            get
            {
                return IsGaragesModified ||
                       IsCarportsModified ||
                       IsOpenSpacesModified;
            }
        }
        
        public void Copy(CarParking newCarParking)
        {
            if (newCarParking == null)
            {
                throw new ArgumentNullException("newCarParking");
            }

            if (newCarParking.IsGaragesModified)
            {
                Garages = newCarParking.Garages;
            }

            if (newCarParking.IsCarportsModified)
            {
                Carports = newCarParking.Carports;
            }

            if (newCarParking.IsOpenSpacesModified)
            {
                OpenSpaces = newCarParking.OpenSpaces;
            }
        }

        public void ClearAllIsModified()
        {
            IsGaragesModified = false;
            IsCarportsModified = false;
            IsOpenSpacesModified = false;
        }
    }
}