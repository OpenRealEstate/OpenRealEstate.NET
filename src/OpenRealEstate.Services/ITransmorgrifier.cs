using OpenRealEstate.Core;

namespace OpenRealEstate.Services
{
    public interface ITransmorgrifier
    {
        /// <summary>
        /// Parses and converts some given data into a listing instance.
        /// </summary>
        /// <param name="data">some data source, like Xml data or json data.</param>
        /// <param name="existingListing">An optional destination listing which will extract any data, into.</param>
        /// <param name="areBadCharactersRemoved">Help clean up the data.</param>
        /// <returns>List of listings, unhandled data and/or errors.</returns>
        /// <remarks>Why does <code>isClearAllIsModified</code> default to <code>false</code>? Because when you generally load some data into a new listing instance, you want to see which properties </remarks>
        ParsedResult Parse(string data,
                           Listing existingListing = null,
                           bool areBadCharactersRemoved = false);
    }
}