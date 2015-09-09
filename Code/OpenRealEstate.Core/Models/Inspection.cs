using System;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class Inspection
    {
        private const string ClosesOnName = "ClosesOn";
        private const string OpensOnName = "OpensOn";
        private readonly DateTimeNullableNotified _closesOn;
        private readonly DateTimeNotified _opensOn;

        public Inspection()
        {
            ModifiedData = new ModifiedData(GetType());

            _closesOn = new DateTimeNullableNotified(ClosesOnName);
            _closesOn.PropertyChanged += ModifiedData.OnPropertyChanged;

            _opensOn = new DateTimeNotified(OpensOnName);
            _opensOn.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }

        public DateTime OpensOn
        {
            get { return _opensOn.Value; }
            set { _opensOn.Value = value; }
        }

        [Obsolete]
        public bool IsOpensOnModified { get; private set; }

        public DateTime? ClosesOn
        {
            get { return _closesOn.Value; }
            set { _closesOn.Value = value; }
        }

        [Obsolete]
        public bool IsClosesOnModified { get; private set; }

        public void Copy(Inspection inspection)
        {
            ModifiedData.Copy(inspection, this);
        }

        public void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedProperties(new[] {ClosesOnName, OpensOnName});
        }
    }
}