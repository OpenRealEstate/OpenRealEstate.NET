namespace OpenRealEstate.Core.Primitives
{
    public class InstanceObjectNotified<T> : BaseNotified
        where T : class, new()
    {
        private T _value;

        public InstanceObjectNotified(string propertyName)
            : base(propertyName)
        {
        }

        public T Value
        {
            get { return _value; }
            set
            {
                if ((_value == null &&
                    value == null) ||
                    (_value != null &&
                    _value.Equals(value)))
                {
                    return;
                }
                _value = value;
                OnPropertyChanged(PropertyName);
            }
        }
    }
}