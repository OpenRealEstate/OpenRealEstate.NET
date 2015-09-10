using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models.Land
{
    public class LandEstate : BaseModifiedData
    {
        private const string NameName = "Name";
        private const string StageName = "Stage";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _name;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _stage;

        public LandEstate()
        {
            _name = new StringNotified(NameName);
            _name.PropertyChanged += ModifiedData.OnPropertyChanged;

            _stage = new StringNotified(StageName);
            _stage.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public string Name
        {
            get { return _name.Value; }
            set { _name.Value = value; }
        }

        public string Stage
        {
            get { return _stage.Value; }
            set { _stage.Value = value; }
        }

        public void Copy(LandEstate newLandEstate, bool isModifiedPropertiesOnly = true)
        {
            if (newLandEstate == null)
            {
                throw new ArgumentNullException("newLandEstate");
            }

            ModifiedData.Copy(newLandEstate, this, isModifiedPropertiesOnly);
        }

        public void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedPropertiesAndCollections();
        }
    }
}