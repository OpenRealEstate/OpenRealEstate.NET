using System.Text;

namespace OpenRealEstate.Services.Extensions
{
    internal static class StringBuilderExtensions
    {
        internal static void PrependWithDelimeter(this StringBuilder stringBuilder,
                                                  string value,
                                                  string delimeter = ", ")
        {
            Guard.AgainstNullOrWhiteSpace(value);

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append(delimeter);
            }

            stringBuilder.Append(value);
        }
    }
}