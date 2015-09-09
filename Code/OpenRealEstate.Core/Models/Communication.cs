using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class Communication : IModifiedData
    {
        private const string DetailsName = "Details";
        private const string CommunicationTypeName = "CommunicationType";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly EnumNotified<CommunicationType> _communicationType;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _details;

        public Communication()
        {
            ModifiedData = new ModifiedData(GetType());

            _details = new StringNotified(DetailsName);
            _details.PropertyChanged += ModifiedData.OnPropertyChanged;

            _communicationType = new EnumNotified<CommunicationType>(CommunicationTypeName);
            _communicationType.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }

        public string Details
        {
            get { return _details.Value; }
            set { _details.Value = value; }
        }

        public CommunicationType CommunicationType
        {
            get { return _communicationType.Value; }
            set { _communicationType.Value = value; }
        }

        public void Copy(Communication newCommunication)
        {
            ModifiedData.Copy(newCommunication, this);
        }

        public void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedPropertiesAndCollections();
        }
    }
}