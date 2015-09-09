namespace OpenRealEstate.Core.Primitives
{
    public class Int32Notified : BaseNotified
    {
        private int _value;

        public Int32Notified(string propertyName) : base(propertyName)
        {
        }

        public int Value
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