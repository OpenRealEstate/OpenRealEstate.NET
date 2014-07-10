using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class Media : IValidate
    {
        public int Order { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
        
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

            if (string.IsNullOrWhiteSpace(Url))
            {
                errors.Add("Url" + keySuffix, "An url is required.");
            }
        }
    }
}