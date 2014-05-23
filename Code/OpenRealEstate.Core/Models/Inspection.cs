using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class Inspection : IValidate
    {
        public DateTime OpensOn { get; set; }
        public DateTime? ClosesOn { get; set; }
        
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

            if (OpensOn == DateTime.MinValue)
            {
                errors.Add("OpensOn" + keySuffix, "The Date/Time value is illegal. Please use a valid value, which is a more current value .. like .. something from this century, please.");
            }

            if (ClosesOn.HasValue &&
                ClosesOn == DateTime.MinValue)
            {
                errors.Add("ClosesOn" + keySuffix, "The Date/Time value is illegal. Please use a valid value, which is a more current value .. like .. something from this century, please, or a NULL value (ie. Not sure when it closes on).");
            }
        }
    }
}