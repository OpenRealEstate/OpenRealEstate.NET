using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;
using NUnit.Framework.Constraints;

namespace OpenRealEstate.Services
{
    public static class XElementExtensions
    {
        public static string Value(this XElement xElement,
            string elementName = null,
            string attributeName = null,
            string attributeValue = null)
        {
            var value = ValueOrDefault(xElement, elementName, attributeName, attributeValue);

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var errorMessage =
                string.Format(
                    "Expected the {0} '{1}' but failed to find it in the element '{2}' or that element exists, but with no data.",
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
            string elementName = null,
            string attributeName = null,
            string attributeValue = null)
        {
            if (xElement == null)
            {
                throw new ArgumentNullException();
            }

            XElement element;
            if (string.IsNullOrWhiteSpace(elementName))
            {
                element = xElement;
            }
            else if (string.IsNullOrWhiteSpace(attributeName) &&
                     string.IsNullOrWhiteSpace(attributeValue))
            {
                element = xElement.Element(elementName);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(attributeValue))
                {
                    // We are trying to find the value of this attribute - so lets get the first element with this attribute.
                    element = xElement
                        .Descendants(elementName)
                        .FirstOrDefault(x => x.Attribute(attributeName) != null);
                }
                else
                {
                    // This is where things get tricky. We need to get the element that contains an attribute name AND attribute value.
                    // For example, an Agent section has 2x <telephone /> elements, but are different by the attributes.
                    // <telephone type="mobile" /> vs <telephone type="BH" />
                    element = xElement
                        .Descendants(elementName)
                        .FirstOrDefault(x => (string) x.Attribute(attributeName) == attributeValue);
                }
            }


            if (element == null)
            {
                return null;
            }

            // This is the next tricky part. Are we after the element value or the attribute value?
            var value = !string.IsNullOrWhiteSpace(attributeName) &&
                string.IsNullOrWhiteSpace(attributeValue)
                ? AttributeValueOrDefault(element, attributeName)
                : element.Value.Trim();

            return string.IsNullOrWhiteSpace(value)
                ? null
                : value;
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
                throw new ArgumentNullException();
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

        public static bool AttributeBoolValueOrDefault(this XElement xElement, string attributeName)
        {
            if (xElement == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrWhiteSpace(attributeName))
            {
                throw new ArgumentNullException();
            }

            var attribute = xElement.Attribute(attributeName);
            if (attribute == null)
            {
                return false;
            }

            // Check to see if this value can be converted to a bool. Ie. 0/1/true/false.
            bool boolValue;
            return bool.TryParse(attribute.Value, out boolValue)
                ? boolValue
                : attribute.Value.ParseOneYesZeroNoToBool();
        }

        public static int IntValueOrDefault(this XElement xElement, string elementName = null)
        {
            var value = xElement.ValueOrDefault(elementName);
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            int number;
            if (int.TryParse(value, out number))
            {
                return number;
            }

            var errorMessage = string.Format("Failed to parse the value '{0}' into an int.", value);
            throw new Exception(errorMessage);
        }

        public static decimal DecimalValueOrDefault(this XElement xElement, string elementName = null)
        {
            var value = xElement.ValueOrDefault(elementName);
            if (string.IsNullOrEmpty(value))
            {
                return 0M;
            }

            // NOTE: This -cannot- handle currencies.
            decimal number;
            if (decimal.TryParse(value, out number))
            {
                return number;
            }

            var errorMessage = string.Format("Failed to parse the value '{0}' into a decimal.", value);
            throw new Exception(errorMessage);
        }

        public static decimal MoneyValueOrDefault(this XElement xElement, CultureInfo cultureInfo, string elementName = null)
        {
            var value = xElement.ValueOrDefault(elementName);
            if (string.IsNullOrEmpty(value))
            {
                return 0M;
            }

            // NOTE: This can now handle values that are either currency or just numbers.
            //       ie. $1000, 1000, etc.
            decimal number;
            if (decimal.TryParse(value,
                NumberStyles.AllowCurrencySymbol | NumberStyles.Number,
                cultureInfo,
                out number))
            {
                return number;
            }

            var errorMessage = string.Format("Failed to parse the value '{0}' into a decimal.", value);
            throw new Exception(errorMessage);
        }

        public static byte ByteValueOrDefault(this XElement xElement, string elementName = null)
        {
            var value = xElement.ValueOrDefault(elementName);
            return value.ParseByteValueOrDefault();
        }

        public static bool BoolValueOrDefault(this XElement xElement, string elementName = null)
        {
            var value = xElement.ValueOrDefault(elementName);
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            // Checking for 0/1/YES/NO
            bool boolValue;
            return bool.TryParse(value, out boolValue)
                ? boolValue
                : value.ParseOneYesZeroNoToBool();
        }

        public static byte BoolOrByteValueOrDefault(this XElement xElement, string elementName = null)
        {
            var value = xElement.ValueOrDefault(elementName);
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            // We're checking to see if the value is YES/NO -before- we do our number check.
            // So the assumption here is that if it's not a YES/NO, then it's a number.
            bool boolValue;
            return value.TryParseYesOrNoToBool(out boolValue)
                ? Convert.ToByte(boolValue)
                : value.ParseByteValueOrDefault();
        }

        public static XElement StripNameSpaces(this XElement root)
        {
            var xElement = new XElement(
                root.Name.LocalName,
                root.HasElements
                    ? root.Elements().Select(StripNameSpaces)
                    : (object) root.Value
                );

            xElement.ReplaceAttributes(root.Attributes()
                .Where(attr => (!attr.IsNamespaceDeclaration)));

            return xElement;
        }
    }
}