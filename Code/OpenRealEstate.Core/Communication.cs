namespace OpenRealEstate.Core
{
    public class Communication
    {
        public string Details { get; set; }

        public CommunicationType CommunicationType { get; set; }

        public override string ToString()
        {
            return $"{CommunicationType} - {(string.IsNullOrWhiteSpace(Details) ? "-no details-" : Details)}";
        }
    }
}