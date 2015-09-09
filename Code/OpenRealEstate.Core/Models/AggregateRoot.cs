using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public abstract class AggregateRoot
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _id;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DateTimeNotified _updatedOn;

        protected AggregateRoot()
        {
            ModifiedData = new ModifiedData(GetType());

            _id = new StringNotified("Id");
            _id.PropertyChanged += ModifiedData.OnPropertyChanged;

            _updatedOn = new DateTimeNotified("UpdatedOn");
            _updatedOn.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }

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

        public void Copy(AggregateRoot newAggregateRoot)
        {
            ModifiedData.Copy(newAggregateRoot, this);
        }

        public virtual void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedProperties(new[] {"Id", "UpdatedOn"});
        }
    }
}