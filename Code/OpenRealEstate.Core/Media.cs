using System;

namespace OpenRealEstate.Core
{
    public class Media
    {
        public DateTime? CreatedOn { get; set; }

        public int Order { get; set; }

        public string Url { get; set; }

        public string Tag { get; set; }

        public override string ToString()
        {
            return $"{Order} : {(string.IsNullOrWhiteSpace(Url) ? "-no url-" : Url)}";
        }
    }
}