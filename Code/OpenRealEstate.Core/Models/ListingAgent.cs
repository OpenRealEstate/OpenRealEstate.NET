using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class ListingAgent : BaseModifiedData
    {
        private const string CommunicationsName = "Communications";
        private const string NameName = "Name";
        private const string OrderName = "Order";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableCollection<Communication> _communications;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _name;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32Notified _order;

        public ListingAgent()
        {
            _communications = new ObservableCollection<Communication>();
            _communications.CollectionChanged +=
                (sender, args) => { ModifiedData.OnCollectionChanged(CommunicationsName); };

            _name = new StringNotified(NameName);
            _name.PropertyChanged += ModifiedData.OnPropertyChanged;

            _order = new Int32Notified(OrderName);
            _order.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public string Name
        {
            get { return _name.Value; }
            set { _name.Value = value; }
        }

        public ReadOnlyCollection<Communication> Communications
        {
            get { return _communications.ToList().AsReadOnly(); }
            set { HelperUtilities.SetCollection(_communications, value, AddCommunications); }
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
                _communications.Add(communication);
            }
        }

        public void RemoveCommunication(Communication communication)
        {
            if (communication == null)
            {
                throw new ArgumentNullException("communication");
            }

            if (_communications != null)
            {
                _communications.Remove(communication);
            }
        }

        public void Copy(ListingAgent newListingAgent,
            CopyDataOptions copyDataOptions = CopyDataOptions.OnlyCopyModifiedProperties)
        {
            ModifiedData.Copy(newListingAgent, this, copyDataOptions);

            if (newListingAgent.ModifiedData.ModifiedCollections.Contains(CommunicationsName))
            {
                _communications.Clear();

                if (newListingAgent.Communications.Any())
                {
                    var communications = new List<Communication>();
                    foreach (var communication in newListingAgent.Communications)
                    {
                        var newCommunication = new Communication();
                        newCommunication.Copy(communication, CopyDataOptions.CopyAllData);
                        communications.Add(newCommunication);
                    }
                    AddCommunications(communications);
                }
            }
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            HelperUtilities.ClearAllObservableCollectionItems(_communications);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} - C: {2}",
                Order,
                string.IsNullOrWhiteSpace(Name)
                    ? "-no name-"
                    : Name,
                Communications != null &&
                Communications.Any()
                    ? Communications.Count
                    : 0);
        }
    }
}