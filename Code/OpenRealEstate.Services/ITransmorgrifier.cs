using System.Collections.Generic;

namespace OpenRealEstate.Services
{
    public interface ITransmorgrifier
    {
        /// <summary>
        /// Converts some given data into a listing instance.
        /// </summary>
        /// <param name="data">some data source, like Xml data or json data.</param>
        /// <param name="areBadCharactersRemoved">Help clean up the data.</param>
        /// <returns>List of listings and any unhandled data..</returns>
        ConvertToResult ConvertTo(string data, bool areBadCharactersRemoved = false);
    }
}