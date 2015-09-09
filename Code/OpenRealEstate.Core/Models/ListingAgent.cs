using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class ListingAgent
    {
        private readonly ObservableCollection<Communication> _communiations;
        private readonly StringNotified _name;
        private readonly Int32Notified _order;
        private const string CommunicationsName = "Communications";
        private const string NameName = "Name";
        private const string OrderName = "Order";
        public ListingAgent()
        {
            ModifiedData = new ModifiedData(GetType());

            _communiations = new ObservableCollection<Communication>();
            _communiations.CollectionChanged += (sender, args) => { ModifiedData.OnCollectionChanged(CommunicationsName); };

            _name = new StringNotified(NameName);
            _name.PropertyChanged += ModifiedData.OnPropertyChanged;

            _order = new Int32Notified(OrderName);
            _order.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }

        public string Name
        {
            get { return _name.Value; }
            set { _name.Value = value; }
        }

        [Obsolete]
        public bool IsNameModified { get; private set; }

        public ReadOnlyCollection<Communication> Communications
        {
            get { return _communiations.ToList().AsReadOnly(); }
        }
        [Obsolete]
        public bool IsCommunicationsModified { get; private set; }

        public int Order
        {
            get { return _order.Value; }
            set { _order.Value = value; }
        }

        [Obsolete]
        public bool IsOrderModified { get; private set; }

        public void AddCommunications(ICollection<Communication> communications)
        {
            if (communications == null)
            {
                throw new ArgumentNullException("communications");
            }

            if (!communications.Any())
            {
                throw new ArgumentOutOfRangeException("agencommunicationsts");
            }

            foreach (var communication in communications)
            {
                _communiations.Add(communication);
            }
        }

        public void RemoveCommunication(Communication communication)
        {
            if (communication == null)
            {
                throw new ArgumentNullException("communication");
            }

            if (_communiations != null)
            {
                _communiations.Remove(communication);
            }
        }

        public void Copy(ListingAgent newListingAgent)
        {
            ModifiedData.Copy(newListingAgent, this);

            if (newListingAgent.ModifiedData.ModifiedCollections.Contains(CommunicationsName))
            {
                var communications = new List<Communication>();
                foreach (var communication in newListingAgent.Communications)
                {
                    var newCommunication = new Communication();
                    newCommunication.Copy(communication);
                    communications.Add(newCommunication);
                }
                AddCommunications(communications);
            }
        }

        public void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedProperties(new[]
            {
                NameName, 
                OrderName,
                CommunicationsName
            });
        }
    }
}