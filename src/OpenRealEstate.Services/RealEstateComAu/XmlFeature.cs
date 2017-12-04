namespace OpenRealEstate.Services.RealEstateComAu
{
    public class XmlFeature
    {
        public XmlFeature(string xmlField,
                          string displayName,
                          bool indoor,
                          bool outdoor,
                          bool eco)
        {
            Guard.AgainstNullOrWhiteSpace(xmlField);
            Guard.AgainstNullOrWhiteSpace(displayName);

            XmlField = xmlField;
            DisplayName = displayName;
            Indoor = indoor;
            Outdoor = outdoor;
            Eco = eco;
        }

        public string XmlField { get; }
        public string DisplayName { get; }
        public bool Indoor { get; }
        public bool Outdoor { get; }
        public bool Eco { get; }
    }
}