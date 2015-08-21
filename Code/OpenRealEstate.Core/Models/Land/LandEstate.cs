using System;

namespace OpenRealEstate.Core.Models.Land
{
    public class LandEstate
    {
        private string _name;
        private string _stage;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                IsNameModified = true;
            }
        }

        public bool IsNameModified { get; set; }

        public string Stage
        {
            get { return _stage; }
            set
            {
                _stage = value;
                IsStageModified = true;
            }
        }

        public bool IsStageModified { get; set; }

        public bool IsModified
        {
            get
            {
                return IsNameModified ||
                       IsStageModified;
            }
        }

        public void Copy(LandEstate newLandEstate)
        {
            if (newLandEstate == null)
            {
                throw new ArgumentNullException("newLandEstate");
            }

            if (newLandEstate.IsNameModified)
            {
                Name = newLandEstate.Name;
            }

            if (newLandEstate.IsStageModified)
            {
                Stage = newLandEstate.Stage;
            }
        }

        public void ClearAllIsModified()
        {
            IsNameModified = false;
            IsStageModified = false;
        }
    }
}