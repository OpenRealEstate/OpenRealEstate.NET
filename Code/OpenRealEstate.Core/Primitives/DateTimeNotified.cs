using System;

namespace OpenRealEstate.Core.Primitives
{
    public class DateTimeNotified : BaseNotified
    {
        private DateTime _value;

        public DateTimeNotified(string propertyName) : base(propertyName)
        {
        }

        public DateTime Value
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