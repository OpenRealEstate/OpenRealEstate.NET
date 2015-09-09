using System;

namespace OpenRealEstate.Core.Primitives
{
    public class EnumNotified<T> : BaseNotified where T : struct, IConvertible
    {
        private T _value;

        public EnumNotified(string propertyName)
            : base(propertyName)
        {
        }

        public T Value
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