using System;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class CarParking
    {
        private readonly Int32Notified _carports;
        private readonly Int32Notified _garages;
        private readonly Int32Notified _openspaces;
        private const string CarportsName = "Carports";
        private const string GaragesName = "Garages";
        private const string OpenSpacesName = "OpenSpaces";

        public CarParking()
        {
            ModifiedData = new ModifiedData(GetType());

            _carports = new Int32Notified(CarportsName);
            _carports.PropertyChanged += ModifiedData.OnPropertyChanged;

            _garages = new Int32Notified(GaragesName);
            _garages.PropertyChanged += ModifiedData.OnPropertyChanged;

            _openspaces = new Int32Notified(OpenSpacesName);
            _openspaces.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }
        public int Garages
        {
            get { return _garages.Value; }
            set { _garages.Value = value; }
        }

        [Obsolete]
        public bool IsGaragesModified { get; set; }

        public int Carports
        {
            get { return _carports.Value; }
            set { _carports.Value = value; }
        }
        [Obsolete]
        public bool IsCarportsModified { get; set; }

        public int OpenSpaces
        {
            get { return _openspaces.Value; }
            set { _openspaces.Value = value; }
        }
        [Obsolete]
        public bool IsOpenSpacesModified { get; set; }

        public int TotalCount
        {
            get { return Garages + Carports + OpenSpaces; }
        }

        public bool IsModified
        {
            get { return ModifiedData.IsModified; }
        }
        
        public void Copy(CarParking newCarParking)
        {
            ModifiedData.Copy(newCarParking, this);
        }

        public void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedProperties(new[]
            {
                CarportsName,
                GaragesName,
                OpenSpacesName
            });
        }
    }
}