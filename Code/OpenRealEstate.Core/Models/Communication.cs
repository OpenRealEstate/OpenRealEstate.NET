using System;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class Communication
    {
        private const string DetailsName = "Details";
        private const string CommunicationTypeName = "CommunicationType";
        private readonly EnumNotified<CommunicationType> _communicationType;
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

        [Obsolete]
        public bool IsDetailsModified { get; private set; }

        public CommunicationType CommunicationType
        {
            get { return _communicationType.Value; }
            set { _communicationType.Value = value; }
        }

        [Obsolete]
        public bool IsCommunicationTypeModified { get; private set; }

        public bool IsModified
        {
            get { return ModifiedData.IsModified; }
        }

        public void Copy(Communication newCommunication)
        {
            ModifiedData.Copy(newCommunication, this);
        }

        public void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedProperties(new[] {DetailsName, CommunicationTypeName});
        }
    }
}