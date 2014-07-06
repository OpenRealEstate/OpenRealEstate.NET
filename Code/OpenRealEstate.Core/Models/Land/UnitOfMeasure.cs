namespace OpenRealEstate.Core.Models.Land
{
    public class UnitOfMeasure
    {
        public string Type { get; set; }
        public decimal Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}",
                Value,
                string.IsNullOrWhiteSpace(Type)
                    ? "-no type-"
                    : Type);
        }
    }
}