namespace OpenRealEstate.Services
{
    public interface ITransmorgrifier
    {
        /// <summary>
        /// Converts some given data into a listing instance.
        /// </summary>
        /// <param name="data">some data source, like Xml data or json data.</param>
        /// <param name="areBadCharactersRemoved">Help clean up the data.</param>
        /// <param name="isClearAllIsModified">After the data is loaded, do we clear all IsModified fields so it looks like the listing(s) are all ready to be used and/or compared against other listings.</param>
        /// <returns>List of listings and any unhandled data.</returns>
        /// <remarks>Why does <code>isClearAllIsModified</code> default to <code>false</code>? Because when you generally load some data into a new listing instance, you want to see which properties </remarks>
        ConvertToResult ConvertTo(string data,
            bool areBadCharactersRemoved = false,
            bool isClearAllIsModified = false);
    }
}