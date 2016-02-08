using System.Xml.Linq;

namespace OpenRealEstate.Services.RealEstateComAu
{
    // Notes.
    /* CategoryType's
     * ^^^^^^^^^^^^^^
     * BUS Business For Sale
     * COM Commercial
     * CLD Commercial Land For Sale
     * LAN Land For Sale
     * REN Home For Rent
     * RUR Rural
     * SAL Home For Sale
     * HOL Holiday Rental
     * CLR Clearing Sale
     * LVS Livestock for Sale
     * LVW Livestock Wanted
     * ALP Agistment
     * STU Stud Stock
     */

    public enum CategoryType
    {
        Unknown,
        Sale,
        Rent,
        Land,
        Rural,
        HolidayRental,
        Commercial,
        CommericalLandForSale,
        BusinessForSale
    }

    public static class CategoryTypeExtensions
    {
        public static string ToDescription(this CategoryType value)
        {
            switch (value)
            {
                case CategoryType.Sale:
                    return "Sale";
                case CategoryType.Rent:
                    return "Rent";
                case CategoryType.Land:
                    return "Land";
                case CategoryType.Rural:
                    return "Rural";
                case CategoryType.HolidayRental:
                    return "Holiday Rental";
                case CategoryType.Commercial:
                    return "Commercial";
                case CategoryType.CommericalLandForSale:
                    return "Commerical Land For Sale";
                case CategoryType.BusinessForSale:
                    return "Business For Sale";
                default:
                    return "Unknown";
            }
        }

        public static CategoryType ToCategoryType(this XName name)
        {
            Guard.AgainstNull(name);
            Guard.AgainstNullOrWhiteSpace(name.LocalName);

            switch (name.LocalName.ToUpperInvariant())
            {
                case "RESIDENTIAL":
                    return CategoryType.Sale;
                case "RENTAL":
                    return CategoryType.Rent;
                case "LAND":
                    return CategoryType.Land;
                case "RURAL":
                    return CategoryType.Rural;
                case "COMMERCIAL":
                    return CategoryType.Commercial;
                case "COMMERCIALLAND":
                    return CategoryType.CommericalLandForSale;
                case "BUSINESS":
                    return CategoryType.BusinessForSale;
                case "HOLIDAYRENTAL":
                    return CategoryType.HolidayRental;
                default:
                    return CategoryType.Unknown;
            }
        }
    }
}