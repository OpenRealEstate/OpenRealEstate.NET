using System;

namespace OpenRealEstate.Core
{
    public enum PropertyType
    {
        Unknown,
        Acreage,
        AcreageSemiRural,
        Alpine,
        Apartment,
        BlockOfUnits,
        Flat,
        House,
        Retirement,
        SemiDetached,
        ServicedApartment,
        Studio,
        Terrace,
        Townhouse,
        Unit,
        VacantLand,
        Villa,
        Warehouse,
        Other
    }

    public static class PropertyTypeExtensions
    {
        public static string ToDescription(this PropertyType value)
        {
            switch (value)
            {
                case PropertyType.Acreage:
                    return "Acreage";
                case PropertyType.AcreageSemiRural:
                    return "Acreage Semi Rural";
                case PropertyType.Alpine:
                    return "Alpine";
                case PropertyType.Apartment:
                    return "Apartment";
                case PropertyType.BlockOfUnits:
                    return "Block Of Units";
                case PropertyType.Flat:
                    return "Flat";
                case PropertyType.House:
                    return "House";
                case PropertyType.Retirement:
                    return "Retirement";
                case PropertyType.SemiDetached:
                    return "SemiDetached";
                case PropertyType.ServicedApartment:
                    return "Serviced Apartment";
                case PropertyType.Studio:
                    return "Studio";
                case PropertyType.Terrace:
                    return "Terrace";
                case PropertyType.Townhouse:
                    return "Townhouse";
                case PropertyType.Unit:
                    return "Unit";
                case PropertyType.VacantLand:
                    return "Vacant Land";
                case PropertyType.Villa:
                    return "Villa";
                case PropertyType.Warehouse:
                    return "Warehouse";
                case PropertyType.Other:
                    return "Other";
                default:
                    return "Unknown";
            }
        }
    }

    public static class PropertyTypeHelpers
    {
        public static PropertyType ToPropertyType(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Equals("ACR", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("ACREAGE", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Acreage;
            }

            if (value.Equals("ASR", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("ACREAGESEMI-RURAL", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("ACREAGESEMIRURAL", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("ACREAGE/SEMI-RURAL", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("ACREAGE SEMI-RURAL", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("ACREAGE SEMIRURAL", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("ACREAGE-SEMI-RURAL", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.AcreageSemiRural;
            }

            if (value.Equals("ALP", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("ALPINE", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Alpine;
            }

            if (value.Equals("APT", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("APARTMENT", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Apartment;
            }

            if (value.Equals("BOU", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("BLOCKOFUNITS", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("BLOCK OF UNITS", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("UNITBLOCK", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.BlockOfUnits;
            }

            if (value.Equals("FLT", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("FLAT", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Flat;
            }

            if (value.Equals("HSE", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("HOUSE", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.House;
            }

            if (value.Equals("RTM", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("RETIREMENT", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Retirement;
            }

            if (value.Equals("SDC", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("DUPLEXSEMI-DETACHED", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("DUPLEX SEMI-DETACHED", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("DUPLEX-SEMI-DETACHED", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("SEMIDETACHED", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("SEMI-DETACHED", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.SemiDetached;
            }

            if (value.Equals("SAP", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("SERVICEDAPARTMENT", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("SERVICED APARTMENT", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.ServicedApartment;
            }

            if (value.Equals("STD", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("STUDIO", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Studio;
            }

            if (value.Equals("TCE", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("TERRACE", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Terrace;
            }

            if (value.Equals("THS", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("TOWNHOUSE", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Townhouse;
            }

            if (value.Equals("UNT", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("UNIT", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Unit;
            }

            if (value.Equals("LND", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("VACANTLAND", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("VACANT LAND", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.VacantLand;
            }

            if (value.Equals("VIL", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("VILLA", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Villa;
            }

            if (value.Equals("WHS", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("WAREHOUSE", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Warehouse;
            }

            if (value.Equals("OTH", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("OTHER", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Other;
            }

            return PropertyType.Unknown;
        }
    }
}