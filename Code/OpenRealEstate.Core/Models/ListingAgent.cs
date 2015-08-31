using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenRealEstate.Core.Models
{
    public class ListingAgent
    {
        private IList<Communication> _communiations;
        private string _name;
        private int _order;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                IsNameModified = true;
            }
        }

        public bool IsNameModified { get; private set; }

        public IList<Communication> Communications
        {
            get { return _communiations; }
            set
            {
                _communiations = value;
                IsCommunicationsModified = true;
            }
        }

        public bool IsCommunicationsModified { get; private set; }

        public int Order
        {
            get { return _order; }
            set
            {
                _order = value;
                IsOrderModified = true;
            }
        }

        public bool IsOrderModified { get; private set; }

        public void Copy(ListingAgent newListingAgent)
        {
            if (newListingAgent == null)
            {
                throw new ArgumentNullException("newListingAgent");
            }

            if (newListingAgent.IsNameModified)
            {
                Name = newListingAgent.Name;
            }

            if (newListingAgent.IsCommunicationsModified)
            {
                if (newListingAgent.Communications == null)
                {
                    Communications = newListingAgent.Communications;
                }
                else
                {
                    Communications = new List<Communication>();
                    foreach (var newCommunication in newListingAgent.Communications)
                    {
                        var communication = new Communication();
                        communication.Copy(newCommunication);
                        Communications.Add(communication);
                    }
                }
            }

            if (newListingAgent.IsOrderModified)
            {
                Order = newListingAgent.Order;
            }
        }

        public void ClearAllIsModified()
        {
            if (Communications != null)
            {
                foreach (var communication in Communications.Where(communication => communication.IsModified))
                {
                    communication.ClearAllIsModified();
                }
            }

            IsNameModified = false;
            IsCommunicationsModified = false;
            IsOrderModified = false;
        }
    }
}