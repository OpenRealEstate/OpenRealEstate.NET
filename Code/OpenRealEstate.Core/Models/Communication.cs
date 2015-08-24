using System;

namespace OpenRealEstate.Core.Models
{
    public class Communication
    {
        private CommunicationType _communicationType;
        private string _details;

        public string Details
        {
            get { return _details; }
            set
            {
                _details = value;
                IsDetailsModified = true;
            }
        }

        public bool IsDetailsModified { get; private set; }

        public CommunicationType CommunicationType
        {
            get { return _communicationType; }
            set
            {
                _communicationType = value;
                IsCommunicationTypeModified = true;
            }
        }

        public bool IsCommunicationTypeModified { get; private set; }

        public void Copy(Communication newCommunication)
        {
            if (newCommunication == null)
            {
                throw new ArgumentNullException("newCommunication");
            }

            if (newCommunication.IsDetailsModified)
            {
                Details = newCommunication.Details;
            }

            if (newCommunication.IsCommunicationTypeModified)
            {
                CommunicationType = newCommunication.CommunicationType;
            }
        }

        public void ClearAllIsModified()
        {
            IsDetailsModified = false;
            IsCommunicationTypeModified = false;
        }
    }
}