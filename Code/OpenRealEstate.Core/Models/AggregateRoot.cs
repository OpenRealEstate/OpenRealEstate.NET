using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenRealEstate.Core.Models
{
    public abstract class AggregateRoot : IValidate
    {
        public string Id { get; set; }
        public DateTime UpdatedOn { get; set; }

        public void Validate(Dictionary<string, string> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }

            if (string.IsNullOrWhiteSpace(Id))
            {
                errors.Add("Id", "An Id is required. eg. Raywhite.Kew, Belle.Mosman69, 12345XXAbCdE");
            }

            if (UpdatedOn == DateTime.MinValue)
            {
                errors.Add("UpdatedOn", "A valid UpdatedOn is required. Please use a Date/Time value that is in this decade or so.");
            }

            //if (existingErrors == null || !existingErrors.Any())
            //{
            //    return;
            //}

            //foreach (var keyValue in existingErrors)
            //{
            //    errors.Add(keyValue.Key, keyValue.Value);
            //}
        }
    }
}