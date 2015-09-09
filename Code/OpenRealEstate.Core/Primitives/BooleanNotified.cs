namespace OpenRealEstate.Core.Primitives
{
    public class BooleanNotified : BaseNotified
    {
        private bool _value;

        public BooleanNotified(string propertyName)
            : base(propertyName)
        {
        }

        public bool Value
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