using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public abstract class AggregateRoot : IValidate
    {
        public string Id { get; set; }
        public DateTime UpdatedOn { get; set; }

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

            if (string.IsNullOrWhiteSpace(Id))
            {
                errors.Add("Id" + keySuffix, "An Id is required. eg. Raywhite.Kew, Belle.Mosman69, 12345XXAbCdE");
            }

            if (UpdatedOn == DateTime.MinValue)
            {
                errors.Add("UpdatedOn" + keySuffix,
                    "A valid UpdatedOn is required. Please use a Date/Time value that is in this decade or so.");
            }
        }
    }
}