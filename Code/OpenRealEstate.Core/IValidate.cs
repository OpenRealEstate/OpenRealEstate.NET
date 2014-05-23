using System.Collections.Generic;

namespace OpenRealEstate.Core
{
    public interface IValidate
    {
        void Validate(Dictionary<string, string> errors, string keySuffix = null);
    }
}