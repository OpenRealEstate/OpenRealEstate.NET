using System;

namespace OpenRealEstate.Core.Primitives
{
    public abstract class BaseModifiedData
    {
        private ModifiedData _modifiedData;

        protected Type CurrentType
        {
            get { return GetType(); }
        }

        public ModifiedData ModifiedData
        {
            get { return _modifiedData ?? (_modifiedData = new ModifiedData(CurrentType)); }
        }

        public virtual void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedPropertiesAndCollections();
        }
    }
}