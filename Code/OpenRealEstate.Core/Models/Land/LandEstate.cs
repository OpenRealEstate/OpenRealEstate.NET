using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models.Land
{
    public class LandEstate
    {
        private const string NameName = "Name";
        private const string StageName = "Stage";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _name;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _stage;

        public LandEstate()
        {
            ModifiedData = new ModifiedData(GetType());

            _name = new StringNotified(NameName);
            _name.PropertyChanged += ModifiedData.OnPropertyChanged;

            _stage = new StringNotified(StageName);
            _stage.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }

        public string Name
        {
            get { return _name.Value; }
            set { _name.Value = value; }
        }

        [Obsolete]
        public bool IsNameModified { get; set; }

        public string Stage
        {
            get { return _stage.Value; }
            set { _stage.Value = value; }
        }

        [Obsolete]
        public bool IsStageModified { get; set; }

        public bool IsModified
        {
            get { return ModifiedData.IsModified; }
        }

        public void Copy(LandEstate newLandEstate)
        {
            if (newLandEstate == null)
            {
                throw new ArgumentNullException("newLandEstate");
            }

            ModifiedData.Copy(newLandEstate, this);
        }

        public void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedProperties(new[] {NameName, StageName});
        }
    }
}