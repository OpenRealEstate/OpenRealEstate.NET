using System;
using System.Collections.Generic;

namespace OpenRealEstate.Services.RealEstateComAu
{
    public static class XmlFeatureHelpers
    {
        private static readonly Lazy<HashSet<XmlFeature>> Features =
            new Lazy<HashSet<XmlFeature>>(CreatePossibleBooleanFeatures);

        public static HashSet<XmlFeature> PossibleBooleanFeatures
        {
            get { return Features.Value; }
        }

        private static HashSet<XmlFeature> CreatePossibleBooleanFeatures()
        {
            return new HashSet<XmlFeature>
            {
                new XmlFeature("airConditioning", "Air Conditioning", true, false, false),
                new XmlFeature("alarmSystem", "Alarm System", true, false, false),
                new XmlFeature("balcony", "Balcony", false, true, false),
                new XmlFeature("broadband", "Broadband", false, false, false),
                new XmlFeature("builtInRobes", "Built In Robes", true, false, false),
                new XmlFeature("courtyard", "Courtyard", false, true, false),
                new XmlFeature("deck", "Deck", false, true, false),
                new XmlFeature("dishwasher", "Dishwasher", true, false, false),
                new XmlFeature("ductedCooling", "Ducted Cooling", true, false, false),
                new XmlFeature("ductedHeating", "Ducted Heating", true, false, false),
                new XmlFeature("evaporativeCooling", "Evaporative Cooling", true, false, false),
                new XmlFeature("floorboards", "Floorboards", true, false, false),
                new XmlFeature("fullyFenced", "Fully Fenced", false, false, false),
                new XmlFeature("furnished", "Furnished", true, false, false),
                new XmlFeature("gasHeating", "Gas Heating", true, false, false),
                new XmlFeature("greyWaterSystem", "Grey Water System", false, false, true),
                new XmlFeature("gym", "Gym", true, false, false),
                new XmlFeature("hydronicHeating", "Hydronic Heating", true, false, false),
                new XmlFeature("insideSpa", "Inside Spa", true, false, false),
                new XmlFeature("intercom", "Intercom", true, false, false),
                new XmlFeature("openFirePlace", "Open Fireplace", true, false, false),
                new XmlFeature("outdoorEnt", "Outdoor Entertainment", false, true, false),
                new XmlFeature("outsideSpa", "Outside Spa", false, true, false),
                new XmlFeature("payTV", "Pay TV", true, false, false),
                new XmlFeature("petFriendly", "Pet Friendly", false, false, false),
                new XmlFeature("pool", "Pool", false, true, false),
                new XmlFeature("poolAboveGround", "Pool Above Ground", false, true, false),
                new XmlFeature("poolInGround", "Pool In Ground", false, true, false),
                new XmlFeature("remoteGarage", "Remote Garage", false, true, false),
                new XmlFeature("reverseCycleAircon", "Reverse Cycle Airconditioning", true, false, false),
                new XmlFeature("rumpusRoom", "Rumpus Room", true, false, false),
                new XmlFeature("secureParking", "Secure Parking", false, true, false),
                new XmlFeature("shed", "Shed", false, true, false),
                new XmlFeature("smokers", "Smokers", true, false, false),
                new XmlFeature("solarHotWater", "Solar Hot Water", false, false, true),
                new XmlFeature("solarPanels", "Solar Panels", false, false, true),
                new XmlFeature("spa", "Spa", false, true, false),
                new XmlFeature("splitSystemAirCon", "Split System Airconditioning", true, false, false),
                new XmlFeature("splitSystemHeating", "Split System Heating", true, false, false),
                new XmlFeature("study", "Study", true, false, false),
                new XmlFeature("tennisCourt", "Tennis Court", false, true, false),
                new XmlFeature("vacuumSystem", "Vacuum System", true, false, false),
                new XmlFeature("waterTank", "Water Tank", false, false, true),
                new XmlFeature("workshop", "Workshop", false, true, false)
            };
        }
    }
}