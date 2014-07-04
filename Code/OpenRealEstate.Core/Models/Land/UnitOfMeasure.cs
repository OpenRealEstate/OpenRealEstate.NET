namespace OpenRealEstate.Core.Models.Land
{
    public class UnitOfMeasure
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}",
                string.IsNullOrWhiteSpace(Value)
                    ? "-no value-"
                    : Value,
                string.IsNullOrWhiteSpace(Type)
                    ? "-no type-"
                    : Type);
        }
    }
}