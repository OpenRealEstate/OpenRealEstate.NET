using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class Communication : IValidate
    {
        public string Details { get; set; }
        public CommunicationType CommunicationType { get; set; }

        public void Validate(Dictionary<string, string> errors, string keySuffix = null)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }

            if (string.IsNullOrWhiteSpace(keySuffix))
            {
                throw new ArgumentNullException("keySuffix");
            }

            if (string.IsNullOrWhiteSpace(Details))
            {
                errors.Add("Details" + keySuffix,
                    "A commucation type requires some details. Eg. the actual phone number or the actual email address.");
            }

            if (CommunicationType == CommunicationType.Unknown)
            {
                errors.Add("CommunicationType" + keySuffix, "Please choose any communication type except Unknown.");
            }
        }
    }
}