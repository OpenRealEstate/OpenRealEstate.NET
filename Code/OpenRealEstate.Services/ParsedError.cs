
namespace OpenRealEstate.Services
{
    public class ParsedError
    {
        public ParsedError(string exceptionMessage,
            string invalidData)
        {
            Guard.AgainstNullOrWhiteSpace(exceptionMessage);
            Guard.AgainstNullOrWhiteSpace(invalidData);

            ExceptionMessage = exceptionMessage;
            InvalidData = invalidData;
        }

        /// <summary>
        /// The exception message that occured when trying to parse the data.
        /// </summary>
        public string ExceptionMessage { get; private set; }

        /// <summary>
        /// The invalid data.
        /// </summary>
        public string InvalidData { get; private set; }

        /// <summary>
        /// Agency/Office Identifier where the listing originated from.
        /// </summary>
        /// <remarks>Optional: If this key/element wasn't parsed, then this will be <code>null</code>.</remarks>
        public string AgencyId { get; set; }

        /// <summary>
        /// Unqiue (to the Agency) Listing Identifier.
        /// </summary>
        /// <remarks>Optional: If this key/element wasn't parsed, then this will be <code>null</code>.</remarks>
        public string ListingId { get; set; }
    }
}