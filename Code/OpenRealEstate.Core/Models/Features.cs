using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class Features
    {
        private const string BathroomsName = "Bathrooms";
        private const string BedroomsName = "Bedrooms";
        private const string CarParkingName = "CarParking";
        private const string EnsuitesName = "Ensuites";
        private const string LivingAreasName = "LivingAreas";
        private const string TagsName = "Tags";
        private const string ToiletsName = "Toilets";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32Notified _bathrooms;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32Notified _bedrooms;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<CarParking> _carParking;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32Notified _ensuites;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32Notified _livingAreas;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableCollection<string> _tags;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32Notified _toilets;

        public Features()
        {
            ModifiedData = new ModifiedData(GetType());

            _bathrooms = new Int32Notified(BathroomsName);
            _bathrooms.PropertyChanged += ModifiedData.OnPropertyChanged;

            _bedrooms = new Int32Notified(BedroomsName);
            _bedrooms.PropertyChanged += ModifiedData.OnPropertyChanged;

            _carParking = new InstanceObjectNotified<CarParking>(CarParkingName);
            _carParking.PropertyChanged += ModifiedData.OnPropertyChanged;

            _ensuites = new Int32Notified(EnsuitesName);
            _ensuites.PropertyChanged += ModifiedData.OnPropertyChanged;

            _livingAreas = new Int32Notified(LivingAreasName);
            _livingAreas.PropertyChanged += ModifiedData.OnPropertyChanged;

            _tags = new ObservableCollection<string>();
            _tags.CollectionChanged += (sender, args) => { ModifiedData.OnCollectionChanged(TagsName); };

            _toilets = new Int32Notified(ToiletsName);
            _toilets.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }

        public int Bedrooms
        {
            get { return _bedrooms.Value; }
            set { _bedrooms.Value = value; }
        }

        public int Bathrooms
        {
            get { return _bathrooms.Value; }
            set { _bathrooms.Value = value; }
        }

        public int Toilets
        {
            get { return _toilets.Value; }
            set { _toilets.Value = value; }
        }

        public CarParking CarParking
        {
            get { return _carParking.Value; }
            set { _carParking.Value = value; }
        }

        public int Ensuites
        {
            get { return _ensuites.Value; }
            set { _ensuites.Value = value; }
        }

        public int LivingAreas
        {
            get { return _livingAreas.Value; }
            set { _livingAreas.Value = value; }
        }

        public ReadOnlyCollection<string> Tags
        {
            get { return _tags.ToList().AsReadOnly(); }
        }

        public void AddTags(ICollection<string> tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException("tags");
            }

            if (!tags.Any())
            {
                throw new ArgumentOutOfRangeException("tags");
            }

            foreach (var tag in tags.Where(tag => !_tags.Contains(tag)))
            {
                _tags.Add(tag);
            }
        }

        public void RemoveTag(string tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            if (_tags != null)
            {
                _tags.Remove(tag);
            }
        }

        public void Copy(Features newFeatures)
        {
            ModifiedData.Copy(newFeatures, this);

            if (newFeatures.ModifiedData.ModifiedCollections.Contains(TagsName))
            {
                _tags.Clear();

                if (newFeatures.Tags.Any())
                {
                    var tags = new HashSet<string>();
                    foreach (var tag in newFeatures.Tags)
                    {
                        tags.Add(tag);
                    }
                    AddTags(tags);
                }
            }
        }

        public void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedPropertiesAndCollections();

            if (_carParking.Value != null &&
                _carParking.Value.ModifiedData.IsModified)
            {
                _carParking.Value.ClearAllIsModified();
            }
        }
    }
}