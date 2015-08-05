using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public class Media : IValidate
    {
        private int _order;
        private string _tag;
        private string _url;

        public int Order
        {
            get { return _order; }
            set
            {
                _order = value;
                IsOrderModified = true;
            }
        }

        public bool IsOrderModified { get; private set; }

        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                IsUrlModified = true;
            }
        }

        public bool IsUrlModified { get; private set; }

        public string Tag
        {
            get { return _tag; }
            set
            {
                _tag = value;
                IsTagModified = true;
            }
        }

        public bool IsTagModified { get; private set; }

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

        public void CopyOverNewData(Media newMedia)
        {
            if (newMedia == null)
            {
                throw new ArgumentNullException("newMedia");
            }

            if (newMedia.IsOrderModified)
            {
                Order = newMedia.Order;
            }

            if (newMedia.IsUrlModified)
            {
                Url = newMedia.Url;
            }

            if (newMedia.IsTagModified)
            {
                Tag = newMedia.Tag;
            }
        }

        public void ClearAllIsModified()
        {
            IsOrderModified = false;
            IsUrlModified = false;
            IsTagModified = false;
        }
    }
}