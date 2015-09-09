namespace OpenRealEstate.Core.Primitives
{
    public class StringNotified : BaseNotified
    {
        private string _value;

        public StringNotified(string propertyName) : base(propertyName)
        {
        }

        public string Value
        {
            get { return _value; }
            set
            {
                if ((_value == null &&
                     value == null) ||
                    _value != null &&
                    _value.Equals(value))
                {
                    return;
                }
                _value = value;
                OnPropertyChanged(PropertyName);
            }
        }
    }
}