using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public abstract class AggregateRoot : BaseModifiedData
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _id;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DateTimeNotified _updatedOn;

        private const string IdName = "Id";
        private const string UpdatedOnName = "UpdatedOn";

        protected AggregateRoot()
        {
            _id = new StringNotified(IdName);
            _id.PropertyChanged += ModifiedData.OnPropertyChanged;

            _updatedOn = new DateTimeNotified(UpdatedOnName);
            _updatedOn.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public string Id
        {
            get { return _id.Value; }
            set { _id.Value = value; }
        }

        public DateTime UpdatedOn
        {
            get { return _updatedOn.Value; }
            set { _updatedOn.Value = value; }
        }

        public void Copy(AggregateRoot newAggregateRoot, bool isModifiedPropertiesOnly = true)
        {
            ModifiedData.Copy(newAggregateRoot, this, isModifiedPropertiesOnly);
        }
    }
}