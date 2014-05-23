using System;
using System.Linq;
using System.Xml.Linq;

namespace OpenRealEstate.Services
{
    public static class XElementExtensions
    {
        public static string Value(this XElement xElement,
            string elementName,
            string attributeName = null,
            string attributeValue = null)
        {
            var value = ValueOrDefault(xElement, elementName, attributeName, attributeValue);

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var errorMessage = string.Format("Expected the {0} '{1}' but failed to find it in the element '{2}'.",
                string.IsNullOrWhiteSpace(attributeName) ||
                string.IsNullOrWhiteSpace(attributeValue)
                    ? "element"
                    : "attribute",
                string.IsNullOrWhiteSpace(attributeName) ||
                string.IsNullOrWhiteSpace(attributeValue)
                    ? elementName
                    : attributeName,
                xElement.Name);
            throw new Exception(errorMessage);
        }

        public static string ValueOrDefault(this XElement xElement,
            string elementName,
            string attributeName = null,
            string attributeValue = null)
        {
            if (xElement == null)
            {
                return null;
            }

            var childElement = xElement.Element(elementName);
            if (childElement == null)
            {
                return null;
            }

            // We are either after the value of an attribute OR
            // the element value given an matching attribute AND an attribute value.
            if (!string.IsNullOrEmpty(attributeName))
            {
                var attribute = childElement.Attribute(attributeName);
                if (attribute == null)
                {
                    return null;
                }

                if (string.IsNullOrWhiteSpace(attributeValue))
                {
                    return attribute.Value;
                }

                if (attribute.Value != attributeValue)
                {
                    return null;
                }
            }

            return childElement.Value.Trim();
        }

        public static string AttributeValue(this XElement xElement, string attributeName)
        {
            var value = AttributeValueOrDefault(xElement, attributeName);

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var errorMessage = string.Format("Expected the attribute '{0}' but failed to find it in the element '{1}'.",
                attributeName,
                xElement.Name);
            throw new Exception(errorMessage);
        }

        public static string AttributeValueOrDefault(this XElement xElement, string attributeName)
        {
            if (xElement == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(attributeName))
            {
                throw new ArgumentNullException("attributeName");
            }

            var attribute = xElement.Attribute(attributeName);
            return attribute == null
                ? null 
                : attribute.Value;
        }

        public static int IntValueOrDefault(this XElement xElement, string childElementName = null)
        {
            if (xElement == null)
            {
                return 0;
            }

            // If we don't provide a child element name, then use the current one.
            var childElement = string.IsNullOrEmpty(childElementName)
                ? xElement
                : xElement.Element(childElementName);
            if (childElement == null)
            {
                return 0;
            }

            var value = childElement.Value;
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            int number;
            if (value.Contains("."))
            {
                int.TryParse(value.Substring(0, value.IndexOf(".", StringComparison.Ordinal)), out number);
            }
            else
            {
                int.TryParse(value, out number);
            }

            return number;
        }

        public static decimal DecimalValueOrDefault(this XElement xElement, string elementName)
        {
            if (xElement == null)
            {
                return 0;
            }

            var childElement = xElement.Element(elementName);
            if (childElement == null)
            {
                return 0;
            }

            var value = childElement.Value;
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            decimal number;
            decimal.TryParse(value, out number);

            return number;
        }

        public static byte ByteValueOrDefault(this XElement xElement, string childElementName = null)
        {
            if (xElement == null)
            {
                return 0;
            }

            // If we don't provide a child element name, then use the current one.
            var childElement = string.IsNullOrEmpty(childElementName)
                ? xElement
                : xElement.Element(childElementName);
            if (childElement == null)
            {
                return 0;
            }

            var value = childElement.Value;
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            byte number;
            if (value.Contains("."))
            {
                byte.TryParse(value.Substring(0, value.IndexOf(".", StringComparison.Ordinal)), out number);
            }
            else
            {
                byte.TryParse(value, out number);
            }

            return number;
        }

        public static XElement StripNameSpaces(this XElement root)
        {
            var xElement = new XElement(
                root.Name.LocalName,
                root.HasElements ?
                    root.Elements().Select(StripNameSpaces) :
                    (object)root.Value
            );

            xElement.ReplaceAttributes(root.Attributes()
                .Where(attr => (!attr.IsNamespaceDeclaration)));

            return xElement;
        }
    }
}