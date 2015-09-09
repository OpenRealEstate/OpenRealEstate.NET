using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace OpenRealEstate.Core.Primitives
{
    public class ModifiedData
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<string> _modifiedCollections;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<string> _modifiedProperties;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Type _type;

        public ModifiedData(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            _type = type;

            _modifiedProperties = new List<string>();
            _modifiedCollections = new List<string>();
        }

        public bool IsModified
        {
            get
            {
                return (_modifiedProperties != null &&
                        _modifiedProperties.Any()) ||
                       (_modifiedCollections != null &&
                        _modifiedCollections.Any());
            }
        }

        public ReadOnlyCollection<string> ModifiedProperties
        {
            get { return _modifiedProperties.AsReadOnly(); }
        }

        public ReadOnlyCollection<string> ModifiedCollections
        {
            get { return _modifiedCollections.AsReadOnly(); }
        }

        public void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!_modifiedProperties.Contains(propertyChangedEventArgs.PropertyName))
            {
                _modifiedProperties.Add(propertyChangedEventArgs.PropertyName);
            }
        }

        public void OnCollectionChanged(string propertyName)
        {
            if (!_modifiedCollections.Contains(propertyName))
            {
                _modifiedCollections.Add(propertyName);
            }
        }

        public void Copy<T>(T source, T target) where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            // Find the modified properties from the source.
            var modifiedData = GetModifiedData(source);
            if (modifiedData == null ||
                !modifiedData.IsModified)
            {
                // Nothing to copy.
                return;
            }

            // Get all the properties for the type.
            var properties = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => modifiedData._modifiedProperties.Contains(x.Name))
                .ToList();

            foreach (var property in properties)
            {
                var sourceValue = source.GetType().GetProperty(property.Name).GetValue(source, null);

                // Does this new property have a ModifiedData field/property?
                var childModifiedData = GetModifiedData(sourceValue);
                if (childModifiedData != null)
                {
                    // Do we have a target instance?
                    var copyableTarget = target.GetType().GetProperty(property.Name).GetValue(target, null);
                    if (copyableTarget == null)
                    {
                        // Create a new instance of this 'child' class and make sure we set the original
                        // target property to reference this new instance.
                        copyableTarget = Activator.CreateInstance(sourceValue.GetType());
                        target.GetType().GetProperty(property.Name).SetValue(target, copyableTarget, null);
                    }

                    // Now copy over the data from the source to the target.
                    var targetModifiedData = GetModifiedData(copyableTarget);
                    targetModifiedData.Copy(sourceValue, copyableTarget);
                }
                else
                {
                    // We have a normal value type.
                    target.GetType().GetProperty(property.Name).SetValue(target, sourceValue, null);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0} : P: {1} : C: {2}",
                IsModified,
                _modifiedProperties != null
                    ? _modifiedProperties.Count.ToString()
                    : "-",
                _modifiedCollections != null
                    ? _modifiedCollections.Count.ToString()
                    : "-");
        }

        public void ClearModifiedProperties(ICollection<string> propertyNames)
        {
            if (propertyNames == null)
            {
                throw new ArgumentNullException("propertyNames");
            }

            if (!propertyNames.Any())
            {
                throw new ArgumentOutOfRangeException("propertyNames");
            }

            if (!IsModified)
            {
                // Nothing to clear.
                return;
            }

            foreach (var propertyName in
                propertyNames.Where(propertyName => _modifiedProperties.Contains(propertyName)))
            {
                _modifiedProperties.Remove(propertyName);
            }

            // TODO: Check if the property is of type `ModifiedData` and if so, call the Clear method on that.
            //       Eg. Listings.BuildingData <-- auto clear.
        }

        private static ModifiedData GetModifiedData(object source)
        {
            const BindingFlags bindingFlags = BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance;

            if (source == null)
            {
                return null;
            }

            // Try properties.
            var modifiedData = source.GetType()
                .GetProperties(bindingFlags)
                .Where(x => x.PropertyType == typeof (ModifiedData))
                .Select(x => x.GetValue(source, null))
                .Cast<ModifiedData>()
                .SingleOrDefault();

            if (modifiedData != null)
            {
                return modifiedData;
            }

            // Nope. nothing. lets try fields.
            return source.GetType()
                .GetFields(bindingFlags)
                .Where(x => x.FieldType == typeof (ModifiedData))
                .Select(x => x.GetValue(source))
                .Cast<ModifiedData>()
                .SingleOrDefault();
        }
    }
}