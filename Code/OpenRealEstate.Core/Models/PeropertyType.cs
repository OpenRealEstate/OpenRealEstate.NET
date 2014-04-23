using System;

namespace OpenRealEstate.Core.Models
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
                throw new ArgumentNullException("value");
            }

            switch (value.ToLowerInvariant())
            {
                case "acr":
                case "acreage":
                    return PropertyType.Acreage;
                case "asr":
                case "acreagesemi-rural":
                case "acreagesemirural":
                    return PropertyType.AcreageSemiRural;
                case "alp":
                case "alpine":
                    return PropertyType.Alpine;
                case "apt":
                case "apartment":
                    return PropertyType.Apartment;
                case "bou":
                case "blockofunits":
                    return PropertyType.BlockOfUnits;
                case "flt":
                case "flat":
                    return PropertyType.Flat;
                case "hse":
                case "house":
                    return PropertyType.House;
                case "rtm":
                case "retirement":
                    return PropertyType.Retirement;
                case "sdc":
                case "duplexsemi-detached":
                case "semidetached":
                    return PropertyType.SemiDetached;
                case "sap":
                case "servicedapartment":
                    return PropertyType.ServicedApartment;
                case "std":
                case "studio":
                    return PropertyType.Studio;
                case "tce":
                case "terrace":
                    return PropertyType.Terrace;
                case "ths":
                case "townhouse":
                    return PropertyType.Townhouse;
                case "unt":
                case "unit":
                    return PropertyType.Unit;
                case "lnd":
                case "vacantland":
                    return PropertyType.VacantLand;
                case "vil":
                case "villa":
                    return PropertyType.Villa;
                case "whs":
                case "warehouse":
                    return PropertyType.Warehouse;
                case "oth":
                case "other":
                    return PropertyType.Other;
                default:
                    return PropertyType.Unknown;
            }
        }
    }
}