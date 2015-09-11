using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class Inspection : BaseModifiedData
    {
        private const string ClosesOnName = "ClosesOn";
        private const string OpensOnName = "OpensOn";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DateTimeNullableNotified _closesOn;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DateTimeNotified _opensOn;

        public Inspection()
        {
            _closesOn = new DateTimeNullableNotified(ClosesOnName);
            _closesOn.PropertyChanged += ModifiedData.OnPropertyChanged;

            _opensOn = new DateTimeNotified(OpensOnName);
            _opensOn.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public DateTime OpensOn
        {
            get { return _opensOn.Value; }
            set { _opensOn.Value = value; }
        }

        public DateTime? ClosesOn
        {
            get { return _closesOn.Value; }
            set { _closesOn.Value = value; }
        }

        public void Copy(Inspection inspection, bool isModifiedPropertiesOnly = true)
        {
            ModifiedData.Copy(inspection, this, isModifiedPropertiesOnly);
        }
    }
}