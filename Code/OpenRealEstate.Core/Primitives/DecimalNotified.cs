namespace OpenRealEstate.Core.Primitives
{
    public class DecimalNotified : BaseNotified
    {
        private decimal _value;

        public DecimalNotified(string propertyName)
            : base(propertyName)
        {
        }

        public decimal Value
        {
            get { return _value; }
            set
            {
                if (_value.Equals(value))
                {
                    return;
                }
                _value = value;
                OnPropertyChanged(PropertyName);
            }
        }
    }
}