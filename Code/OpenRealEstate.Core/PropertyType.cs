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

            switch (value.ToUpperInvariant())
            {
                case "ACR":
                case "ACREAGE":
                    return PropertyType.Acreage;
                case "ASR":
                case "ACREAGESEMI-RURAL":
                case "ACREAGESEMIRURAL":
                case "ACREAGE/SEMI-RURAL":
                case "ACREAGE SEMI-RURAL":
                case "ACREAGE SEMIRURAL":
                case "ACREAGE-SEMI-RURAL":
                    return PropertyType.AcreageSemiRural;
                case "ALP":
                case "ALPINE":
                    return PropertyType.Alpine;
                case "APT":
                case "APARTMENT":
                    return PropertyType.Apartment;
                case "BOU":
                case "BLOCKOFUNITS":
                case "BLOCK OF UNITS":
                case "UNITBLOCK":
                    return PropertyType.BlockOfUnits;
                case "FLT":
                case "FLAT":
                    return PropertyType.Flat;
                case "HSE":
                case "HOUSE":
                    return PropertyType.House;
                case "RTM":
                case "RETIREMENT":
                    return PropertyType.Retirement;
                case "SDC":
                case "DUPLEXSEMI-DETACHED":
                case "DUPLEX SEMI-DETACHED":
                case "DUPLEX-SEMI-DETACHED":
                case "SEMIDETACHED":
                case "SEMI-DETACHED":
                    return PropertyType.SemiDetached;
                case "SAP":
                case "SERVICEDAPARTMENT":
                case "SERVICED APARTMENT":
                    return PropertyType.ServicedApartment;
                case "STD":
                case "STUDIO":
                    return PropertyType.Studio;
                case "TCE":
                case "TERRACE":
                    return PropertyType.Terrace;
                case "THS":
                case "TOWNHOUSE":
                    return PropertyType.Townhouse;
                case "UNT":
                case "UNIT":
                    return PropertyType.Unit;
                case "LND":
                case "VACANTLAND":
                case "VACANT LAND":
                    return PropertyType.VacantLand;
                case "VIL":
                case "VILLA":
                    return PropertyType.Villa;
                case "WHS":
                case "WAREHOUSE":
                    return PropertyType.Warehouse;
                case "OTH":
                case "OTHER":
                    return PropertyType.Other;
                default:
                    return PropertyType.Unknown;
            }
        }
    }
}