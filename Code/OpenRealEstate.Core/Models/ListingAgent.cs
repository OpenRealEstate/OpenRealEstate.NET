using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class ListingAgent : IModifiedData
    {
        private const string CommunicationsName = "Communications";
        private const string NameName = "Name";
        private const string OrderName = "Order";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableCollection<Communication> _communiations;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _name;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32Notified _order;

        public ListingAgent()
        {
            ModifiedData = new ModifiedData(GetType());

            _communiations = new ObservableCollection<Communication>();
            _communiations.CollectionChanged +=
                (sender, args) => { ModifiedData.OnCollectionChanged(CommunicationsName); };

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

        public ReadOnlyCollection<Communication> Communications
        {
            get { return _communiations.ToList().AsReadOnly(); }
        }

        public int Order
        {
            get { return _order.Value; }
            set { _order.Value = value; }
        }

        public void AddCommunications(ICollection<Communication> communications)
        {
            if (communications == null)
            {
                throw new ArgumentNullException("communications");
            }

            if (!communications.Any())
            {
                throw new ArgumentOutOfRangeException("communications");
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

        public void Copy(ListingAgent newListingAgent, bool isModifiedPropertiesOnly = true)
        {
            ModifiedData.Copy(newListingAgent, this, isModifiedPropertiesOnly);

            if (newListingAgent.ModifiedData.ModifiedCollections.Contains(CommunicationsName))
            {
                _communiations.Clear();

                if (newListingAgent.Communications.Any())
                {
                    var communications = new List<Communication>();
                    foreach (var communication in newListingAgent.Communications)
                    {
                        var newCommunication = new Communication();
                        newCommunication.Copy(communication, false);
                        communications.Add(newCommunication);
                    }
                    AddCommunications(communications);
                }
            }
        }

        public void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedPropertiesAndCollections();
        }
    }
}