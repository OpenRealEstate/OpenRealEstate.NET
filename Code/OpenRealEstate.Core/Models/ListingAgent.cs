using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenRealEstate.Core.Models
{
    public class ListingAgent : IValidate
    {
        public string Name { get; set; }
        public IList<Communication> Communications { get; set; }
        public int Order { get; set; }

        public void Validate(Dictionary<string, string> errors, string keySuffix = null)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }

            // We can have a string.Empty keySuffix, which means do have a key to postpend.
            if (keySuffix == null)
            {
                throw new ArgumentNullException("keySuffix");
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                errors.Add("Name" + keySuffix, "An agent requires a name.");
            }

            if (Communications != null &&
                Communications.Any())
            {
                foreach (var communication in Communications)
                {
                    communication.Validate(errors, "-" + Name.Replace(' ', '-'));
                }
            }
        }
    }
}