using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class LandDetails
    {
        private const string AreaName = "Area";
        private const string CrossOverName = "CrossOver";
        private const string DepthsName = "Depths";
        private const string FrontageName = "Frontage";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<UnitOfMeasure> _area;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _crossOver;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableCollection<Depth> _depths;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<UnitOfMeasure> _frontage;

        public LandDetails()
        {
            ModifiedData = new ModifiedData(GetType());

            _area = new InstanceObjectNotified<UnitOfMeasure>(AreaName);
            _area.PropertyChanged += ModifiedData.OnPropertyChanged;

            _crossOver = new StringNotified(CrossOverName);
            _crossOver.PropertyChanged += ModifiedData.OnPropertyChanged;

            _depths = new ObservableCollection<Depth>();
            _depths.CollectionChanged += (sender, args) => { ModifiedData.OnCollectionChanged(DepthsName); };

            _frontage = new InstanceObjectNotified<UnitOfMeasure>(FrontageName);
            _frontage.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }

        public UnitOfMeasure Area
        {
            get { return _area.Value; }
            set { _area.Value = value; }
        }

        public UnitOfMeasure Frontage
        {
            get { return _frontage.Value; }
            set { _frontage.Value = value; }
        }

        public ReadOnlyCollection<Depth> Depths
        {
            get { return _depths.ToList().AsReadOnly(); }
        }

        public string CrossOver
        {
            get { return _crossOver.Value; }
            set { _crossOver.Value = value; }
        }

        public void AddDepths(ICollection<Depth> depths)
        {
            if (depths == null)
            {
                throw new ArgumentNullException("depths");
            }

            if (!depths.Any())
            {
                throw new ArgumentOutOfRangeException("depths");
            }

            foreach (var depth in depths)
            {
                _depths.Add(depth);
            }
        }

        public void RemoveDepths(Depth depth)
        {
            if (depth == null)
            {
                throw new ArgumentNullException("depth");
            }

            if (_depths != null)
            {
                _depths.Remove(depth);
            }
        }

        public void Copy(LandDetails newLandDetails)
        {
            if (newLandDetails == null)
            {
                throw new ArgumentNullException("newLandDetails");
            }

            ModifiedData.Copy(newLandDetails, this);

            if (newLandDetails.ModifiedData.ModifiedCollections.Contains(DepthsName))
            {
                _depths.Clear();

                if (newLandDetails.Depths.Any())
                {
                    var depths = new List<Depth>();
                    foreach (var depth in newLandDetails.Depths)
                    {
                        var newDepth = new Depth();
                        newDepth.Copy(depth);
                        depths.Add(newDepth);
                    }
                    AddDepths(depths);
                }
            }
        }

        public void ClearAllIsModified()
        {
            if (_area.Value != null &&
                _area.Value.ModifiedData.IsModified)
            {
                _area.Value.ClearAllIsModified();
            }

            if (_frontage.Value != null &&
                _frontage.Value.ModifiedData.IsModified)
            {
                _frontage.Value.ClearAllIsModified();
            }

            ModifiedData.ClearModifiedPropertiesAndCollections();
        }
    }
}