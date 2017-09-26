using System;

namespace OpenRealEstate.Core
{
    // NOTE: this class doesn't inherit from AggregateRoot because it needs to
    //       support a nullable CreatedOn, while the A.G. class requires a
    //       CreatedOn value.
    public class Media
    {
        public string Id { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int Order { get; set; }

        public string Url { get; set; }

        public string Tag { get; set; }

        public string ContentType { get; set; }

        public override string ToString()
        {
            return $"{Order} : {(string.IsNullOrWhiteSpace(Url) ? "-no url-" : Url)}";
        }
    }
}