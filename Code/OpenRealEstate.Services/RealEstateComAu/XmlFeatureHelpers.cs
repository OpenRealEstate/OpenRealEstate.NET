using System;
using System.Collections.Generic;

namespace OpenRealEstate.Services.RealEstateComAu
{
    public static class XmlFeatureHelpers
    {
        private static readonly Lazy<IDictionary<string, XmlFeature>> Features =
            new Lazy<IDictionary<string, XmlFeature>>(CreatePossibleBooleanFeatures);

        public static IDictionary<string, XmlFeature> PossibleBooleanFeatures
        {
            get { return Features.Value; }
        }

        private static IDictionary<string, XmlFeature> CreatePossibleBooleanFeatures()
        {
            return new Dictionary<string, XmlFeature>
            {
                {"airConditioning", new XmlFeature("airConditioning", "Air Conditioning", true, false, false)},
                {"alarmSystem", new XmlFeature("alarmSystem", "Alarm System", true, false, false)},
                {"balcony", new XmlFeature("balcony", "Balcony", false, true, false)},
                {"broadband", new XmlFeature("broadband", "Broadband", false, false, false)},
                {"builtInRobes", new XmlFeature("builtInRobes", "Built In Robes", true, false, false)},
                {"courtyard", new XmlFeature("courtyard", "Courtyard", false, true, false)},
                {"deck", new XmlFeature("deck", "Deck", false, true, false)},
                {"dishwasher", new XmlFeature("dishwasher", "Dishwasher", true, false, false)},
                {"ductedCooling", new XmlFeature("ductedCooling", "Ducted Cooling", true, false, false)},
                {"ductedHeating", new XmlFeature("ductedHeating", "Ducted Heating", true, false, false)},
                {"evaporativeCooling", new XmlFeature("evaporativeCooling", "Evaporative Cooling", true, false, false)},
                {"floorboards", new XmlFeature("floorboards", "Floorboards", true, false, false)},
                {"fullyFenced", new XmlFeature("fullyFenced", "Fully Fenced", false, false, false)},
                {"furnished", new XmlFeature("furnished", "Furnished", true, false, false)},
                {"gasHeating", new XmlFeature("gasHeating", "Gas Heating", true, false, false)},
                {"greyWaterSystem", new XmlFeature("greyWaterSystem", "Grey Water System", false, false, true)},
                {"gym", new XmlFeature("gym", "Gym", true, false, false)},
                {"hydronicHeating", new XmlFeature("hydronicHeating", "Hydronic Heating", true, false, false)},
                {"insideSpa", new XmlFeature("insideSpa", "Inside Spa", true, false, false)},
                {"intercom", new XmlFeature("intercom", "Intercom", true, false, false)},
                {"openFirePlace", new XmlFeature("openFirePlace", "Open Fireplace", true, false, false)},
                {"outdoorEnt", new XmlFeature("outdoorEnt", "Outdoor Entertainment", false, true, false)},
                {"outsideSpa", new XmlFeature("outsideSpa", "Outside Spa", false, true, false)},
                {"payTV", new XmlFeature("payTV", "Pay TV", true, false, false)},
                {"petFriendly", new XmlFeature("petFriendly", "Pet Friendly", false, false, false)},
                {"pool", new XmlFeature("pool", "Pool", false, true, false)},
                {"poolAboveGround", new XmlFeature("poolAboveGround", "Pool Above Ground", false, true, false)},
                {"poolInGround", new XmlFeature("poolInGround", "Pool In Ground", false, true, false)},
                {"remoteGarage", new XmlFeature("remoteGarage", "Remote Garage", false, true, false)},
                {"reverseCycleAircon",new XmlFeature("reverseCycleAircon", "Reverse Cycle Airconditioning", true, false, false)},
                {"rumpusRoom", new XmlFeature("rumpusRoom", "Rumpus Room", true, false, false)},
                {"secureParking", new XmlFeature("secureParking", "Secure Parking", false, true, false)},
                {"shed", new XmlFeature("shed", "Shed", false, true, false)},
                {"smokers", new XmlFeature("smokers", "Smokers", true, false, false)},
                {"solarHotWater", new XmlFeature("solarHotWater", "Solar Hot Water", false, false, true)},
                {"solarPanels", new XmlFeature("solarPanels", "Solar Panels", false, false, true)},
                {"spa", new XmlFeature("spa", "Spa", false, true, false)},
                {"splitSystemAirCon",new XmlFeature("splitSystemAirCon", "Split System Airconditioning", true, false, false)},
                {"splitSystemHeating", new XmlFeature("splitSystemHeating", "Split System Heating", true, false, false)},
                {"study", new XmlFeature("study", "Study", true, false, false)},
                {"tennisCourt", new XmlFeature("tennisCourt", "Tennis Court", false, true, false)},
                {"vacuumSystem", new XmlFeature("vacuumSystem", "Vacuum System", true, false, false)},
                {"waterTank", new XmlFeature("waterTank", "Water Tank", false, false, true)},
                {"workshop", new XmlFeature("workshop", "Workshop", false, true, false)}
            };
        }
    }
}