using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class Media : IModifiedData
    {
        private const string OrderName = "Order";
        private const string TagName = "Tag";
        private const string UrlName = "Url";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32Notified _order;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _tag;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _url;

        public Media()
        {
            ModifiedData = new ModifiedData(GetType());

            _order = new Int32Notified(OrderName);
            _order.PropertyChanged += ModifiedData.OnPropertyChanged;

            _tag = new StringNotified(TagName);
            _tag.PropertyChanged += ModifiedData.OnPropertyChanged;

            _url = new StringNotified(UrlName);
            _url.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }

        public int Order
        {
            get { return _order.Value; }
            set { _order.Value = value; }
        }

        public string Url
        {
            get { return _url.Value; }
            set { _url.Value = value; }
        }

        public string Tag
        {
            get { return _tag.Value; }
            set { _tag.Value = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}",
                Order,
                string.IsNullOrWhiteSpace(Url)
                    ? "-no url-"
                    : Url);
        }

        public void Copy(Media newMedia, bool isModifiedPropertiesOnly = true)
        {
            ModifiedData.Copy(newMedia, this, isModifiedPropertiesOnly);
        }

        public void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedPropertiesAndCollections();
        }
    }
}