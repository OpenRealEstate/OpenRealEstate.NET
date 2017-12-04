namespace OpenRealEstate.Core
{
    public class CarParking
    {
        public byte Garages { get; set; }

        public byte Carports { get; set; }

        public byte OpenSpaces { get; set; }

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
                var value = Garages + Carports + OpenSpaces;
                return value > byte.MaxValue
                    ? byte.MaxValue
                    : (byte) value;
            }
        }

        public override string ToString()
        {
            return $"C:{Carports} G:{Garages} O:{OpenSpaces} / T:{TotalCount}";
        }
    }
}