using System;

namespace OpenRealEstate.Core.Primitives
{
    public class DateTimeNullableNotified : BaseNotified
    {
        private DateTime? _value;

        public DateTimeNullableNotified(string propertyName)
            : base(propertyName)
        {
        }

        public DateTime? Value
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