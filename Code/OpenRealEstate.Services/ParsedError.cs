using OpenRealEstate.Core;

namespace OpenRealEstate.Services
{
    public class ParsedError
    {
        public ParsedError(string exceptionMessage,
            string invalidData)
        {
            Guard.AgainstNulls(exceptionMessage);
            Guard.AgainstNulls(invalidData);

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
    }
}