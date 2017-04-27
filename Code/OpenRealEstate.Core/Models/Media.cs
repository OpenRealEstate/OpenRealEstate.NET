﻿using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class Media : BaseModifiedData
    {
        private const string CreatedOnName = "CreatedOn";
        private const string OrderName = "Order";
        private const string TagName = "Tag";
        private const string UrlName = "Url";
        private const string ContentTypeName = "ContentType";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DateTimeNullableNotified _createdOn;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32Notified _order;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _tag;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _url;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _contentType;

        public Media()
        {
            _createdOn = new DateTimeNullableNotified(CreatedOnName);
            _createdOn.PropertyChanged += ModifiedData.OnPropertyChanged;

            _order = new Int32Notified(OrderName);
            _order.PropertyChanged += ModifiedData.OnPropertyChanged;

            _tag = new StringNotified(TagName);
            _tag.PropertyChanged += ModifiedData.OnPropertyChanged;

            _url = new StringNotified(UrlName);
            _url.PropertyChanged += ModifiedData.OnPropertyChanged;

            _contentType = new StringNotified(ContentTypeName);
            _contentType.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public DateTime? CreatedOn {
            get { return _createdOn.Value; }
            set { _createdOn.Value = value; }
        }

        public int Order
        {
            get { return _order.Value; }
            set { _order.Value = value; }
        }
        
        public string Tag
        {
            get { return _tag.Value; }
            set { _tag.Value = value; }
        }

        public string Url
        {
            get { return _url.Value; }
            set { _url.Value = value; }
        }
        
        public string ContentType
        {
            get { return _contentType.Value; }
            set { _contentType.Value = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}",
                Order,
                string.IsNullOrWhiteSpace(Url)
                    ? "-no url-"
                    : Url);
        }

        public void Copy(Media newMedia, 
                         CopyDataOptions copyDataOptions = CopyDataOptions.OnlyCopyModifiedProperties)
        {
            ModifiedData.Copy(newMedia, this, copyDataOptions);
        }
    }
}