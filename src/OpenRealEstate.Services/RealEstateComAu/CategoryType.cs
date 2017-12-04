using System;
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

            if (name.LocalName.Equals("RESIDENTIAL", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Sale;
            }

            if (name.LocalName.Equals("RENTAL", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Rent;
            }

            if (name.LocalName.Equals("LAND", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Land;
            }

            if (name.LocalName.Equals("RURAL", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Rural;
            }

            if (name.LocalName.Equals("COMMERCIAL", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.Commercial;
            }

            if (name.LocalName.Equals("COMMERCIALLAND", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.CommericalLandForSale;
            }

            if (name.LocalName.Equals("BUSINESS", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.BusinessForSale;
            }

            if (name.LocalName.Equals("HOLIDAYRENTAL", StringComparison.OrdinalIgnoreCase))
            {
                return CategoryType.HolidayRental;
            }

            return CategoryType.Unknown;
        }
    }
}