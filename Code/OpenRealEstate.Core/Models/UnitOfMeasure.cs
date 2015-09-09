using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class UnitOfMeasure
    {
        private const string TypeName = "Type";
        private const string ValueName = "Value";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _type;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DecimalNotified _value;

        public UnitOfMeasure()
        {
            ModifiedData = new ModifiedData(GetType());

            _type = new StringNotified(TypeName);
            _type.PropertyChanged += ModifiedData.OnPropertyChanged;

            _value = new DecimalNotified(ValueName);
            _value.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }

        public string Type
        {
            get { return _type.Value; }
            set { _type.Value = value; }
        }

        [Obsolete]
        public bool IsTypeModified { get; private set; }

        public decimal Value
        {
            get { return _value.Value; }
            set { _value.Value = value; }
        }

        [Obsolete]
        public bool IsValueModified { get; private set; }

        public bool IsModified
        {
            get { return ModifiedData.IsModified; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}",
                Value,
                string.IsNullOrWhiteSpace(Type)
                    ? "-no type-"
                    : Type);
        }

        public void Copy(UnitOfMeasure newUnitOfMeasure)
        {
            if (newUnitOfMeasure == null)
            {
                throw new ArgumentNullException("newUnitOfMeasure");
            }

            ModifiedData.Copy(newUnitOfMeasure, this);
        }

        public virtual void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedProperties(new[] {TypeName, ValueName});
        }
    }
}