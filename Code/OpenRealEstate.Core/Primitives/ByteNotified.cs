namespace OpenRealEstate.Core.Primitives
{
    public class ByteNotified : BaseNotified
    {
        private byte _value;

        public ByteNotified(string propertyName) : base(propertyName)
        {
        }

        public byte Value
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