using System.Collections.Generic;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Services
{
    public interface ITransmorgrifier
    {
        IList<Listing> Convert(string data);
    }
}