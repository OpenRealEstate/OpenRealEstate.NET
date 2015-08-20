using System;

namespace OpenRealEstate.Core.Models
{
    public abstract class AggregateRoot
    {
        private string _id;
        private DateTime _updatedOn;

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                IsIdModified = true;
            }
        }

        public bool IsIdModified { get; private set; }

        public DateTime UpdatedOn
        {
            get { return _updatedOn; }
            set
            {
                _updatedOn = value;
                IsUpdatedOnModified = true;
            }
        }

        public bool IsUpdatedOnModified { get; private set; }

        public virtual void CopyOverNewData(AggregateRoot newAggregateRoot)
        {
            if (newAggregateRoot == null)
            {
                throw new ArgumentNullException("newAggregateRoot");
            }

            if (newAggregateRoot.IsIdModified)
            {
                Id = newAggregateRoot.Id;
            }

            if (newAggregateRoot.IsUpdatedOnModified)
            {
                UpdatedOn = newAggregateRoot.UpdatedOn;
            }
        }

        public virtual void ClearAllIsModified()
        {
            IsIdModified = false;
            IsUpdatedOnModified = false;
        }
    }
}