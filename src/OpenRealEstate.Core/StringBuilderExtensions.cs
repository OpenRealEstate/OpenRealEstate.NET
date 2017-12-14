using System.Text;

namespace OpenRealEstate.Core
{
    internal static class StringBuilderExtensions
    {
        internal static void PrependWithDelimeter(this StringBuilder stringBuilder,
                                                  string value,
                                                  string delimeter = ", ")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new System.ArgumentException(nameof(value));
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append(delimeter);
            }

            stringBuilder.Append(value);
        }
    }
}