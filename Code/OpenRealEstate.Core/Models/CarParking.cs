using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class CarParking : BaseModifiedData
    {
        private const string CarportsName = "Carports";
        private const string GaragesName = "Garages";
        private const string OpenSpacesName = "OpenSpaces";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ByteNotified _carports;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ByteNotified _garages;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ByteNotified _openspaces;

        public CarParking()
        {
            _carports = new ByteNotified(CarportsName);
            _carports.PropertyChanged += ModifiedData.OnPropertyChanged;

            _garages = new ByteNotified(GaragesName);
            _garages.PropertyChanged += ModifiedData.OnPropertyChanged;

            _openspaces = new ByteNotified(OpenSpacesName);
            _openspaces.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public byte Garages
        {
            get { return _garages.Value; }
            set { _garages.Value = value; }
        }

        public byte Carports
        {
            get { return _carports.Value; }
            set { _carports.Value = value; }
        }

        public byte OpenSpaces
        {
            get { return _openspaces.Value; }
            set { _openspaces.Value = value; }
        }

        /// <summary>
        /// NOTICE: This is the sum of Garages, Carports and Openspaces. If the sum of all three are greater than byte.MaxValue, then the total count is set to byte.MaxValue.<br/>
        ///         This is to avoid stilly data entry edgecases which cause overflow errors.<br/>
        ///         The provided Car Parking Validator does a check for this and throws a validation error if the sum of all three, overflows.<br/>
        ///         Eg. G:100 + C:100 + O:100 == T:255, not 300. 
        /// </summary>
        public byte TotalCount
        {
            get
            {
                int value = Garages + Carports + OpenSpaces;
                return value > byte.MaxValue
                    ? byte.MaxValue
                    : (byte) value;
            }
        }

        public void Copy(CarParking newCarParking,
            CopyDataOptions copyDataOptions = CopyDataOptions.OnlyCopyModifiedProperties)
        {
            ModifiedData.Copy(newCarParking, this, copyDataOptions);
        }

        public override string ToString()
        {
            return string.Format("C:{0} G:{1} O:{2} / T:{3}",
                Carports,
                Garages,
                OpenSpaces,
                TotalCount);
        }
    }
}