using System.Collections.Generic;

namespace OpenRealEstate.Core
{
    public interface IValidate
    {
        //Dictionary<string, string> Validate();
        void Validate(Dictionary<string, string> errors);
    }
}