using System;
using System.ComponentModel;

namespace OpenRealEstate.Core.Primitives
{
    public abstract class BaseNotified : INotifyPropertyChanged
    {
        protected BaseNotified(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            PropertyName = propertyName;
        }

        protected string PropertyName { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}