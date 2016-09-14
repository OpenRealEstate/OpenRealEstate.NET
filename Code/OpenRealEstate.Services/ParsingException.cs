using System;

namespace OpenRealEstate.Services
{
    internal class ParsingException : Exception
    {
        public ParsingException(string message,
            string agencyId,
            string listingId,
            Exception exception) : base(message, exception)
        {
            Guard.AgainstNullOrWhiteSpace(message);
            Guard.AgainstNull(exception);

            AgencyId = agencyId;
            ListingId = listingId;
        }

        public string AgencyId { get; private set; }
        public string ListingId { get; private set; }
    }
}