using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class Depth : UnitOfMeasure
    {
        private const string SideName = "Side";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _side;

        public Depth()
        {
            _side = new StringNotified(SideName);
            _side.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public string Side
        {
            get { return _side.Value; }
            set { _side.Value = value; }
        }
    }
}