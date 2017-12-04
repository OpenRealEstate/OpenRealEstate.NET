using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using OpenRealEstate.Core;
using System.Collections.Generic;
using System;

namespace OpenRealEstate.FakeData
{
    public class FakeListingAgent
    {
        public static ListingAgent CreateAFakeListingAgent()
        {
            var communications = new List<Communication>
            {
                FakeCommunication.CreateAFakeCommunication()
            };

            if (GetRandom.Int(1, 5) == 1)
            {
                communications.Add(FakeCommunication.CreateAFakeCommunication(CommunicationType.Email));
            }

            return Builder<ListingAgent>.CreateNew()
                                        .With(x => x.Name, $"{GetRandom.FirstName()} {GetRandom.LastName()}")
                                        .With(x => x.Communications, communications)
                                        .Build();
        }

        public static IEnumerable<ListingAgent> CreateFakeListingAgents(int numberOfAgents = 1)
        {
            if (numberOfAgents <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfAgents));
            }

            var agents = new List<ListingAgent>
            {
                CreateAFixedListingAgent()
            };

            // Start at 1 because the first listing should be the hard-coded one.
            for (var i = 1; i < numberOfAgents; i++)
            {
                agents.Add(CreateAFakeListingAgent());
            }

            return agents;
        }

        public static ListingAgent CreateAFixedListingAgent(string name = "Mr. John Doe",
                                                            string email = "jdoe@somedomain.com.au",
                                                            string mobilePhone = "0418 123 456",
                                                            string workPhone = "05 1234 5678",
                                                            int order = 1)
        {
            var emailCommunocation = new Communication
            {
                CommunicationType = CommunicationType.Email,
                Details = email
            };

            var mobileCommunication = new Communication
            {
                CommunicationType = CommunicationType.Mobile,
                Details = mobilePhone
            };

            var workCommunication = new Communication
            {
                CommunicationType = CommunicationType.Landline,
                Details = workPhone
            };

            return new ListingAgent
            {
                Name = name,
                Order = order,
                Communications = new List<Communication>
                {
                    emailCommunocation,
                    mobileCommunication,
                    workCommunication
                }
            };
        }
    }
}
