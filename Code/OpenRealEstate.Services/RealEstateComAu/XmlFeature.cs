using OpenRealEstate.Core;

namespace OpenRealEstate.Services.RealEstateComAu
{
    public class XmlFeature
    {
        public XmlFeature(string xmlField, string displayName, bool indoor, bool outdoor, bool eco)
        {
            Guard.AgainstNullOrWhiteSpace(xmlField);
            Guard.AgainstNullOrWhiteSpace(displayName);

            XmlField = xmlField;
            DisplayName = displayName;
            Indoor = indoor;
            Outdoor = outdoor;
            Eco = eco;
        }

        public string XmlField { get; private set; }
        public string DisplayName { get; private set; }
        public bool Indoor { get; private set; }
        public bool Outdoor { get; private set; }
        public bool Eco { get; private set; }
    }
}