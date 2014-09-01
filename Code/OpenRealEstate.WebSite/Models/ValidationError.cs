using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace OpenRealEstate.WebSite.Models
{
    public class ValidationError
    {
        public ValidationError(string id,
            ValidationFailure validationFailure)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentOutOfRangeException("id");
            }

            if (validationFailure == null)
            {
                throw new ArgumentNullException("validationFailure");
            }

            Id = id;
            ValidationFailure = validationFailure;
        }

        public string Id { get; private set; }
        public ValidationFailure ValidationFailure { get; private set; }

        public static IList<ValidationError> ConvertToValidationErrors(string id,
            IList<ValidationFailure> validationFailures)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentOutOfRangeException("id");
            }

            if (validationFailures == null)
            {
                throw new ArgumentNullException("validationFailures");
            }

            return (from vf in validationFailures
                select new ValidationError(id, vf))
                .ToList();
        }
    }
}