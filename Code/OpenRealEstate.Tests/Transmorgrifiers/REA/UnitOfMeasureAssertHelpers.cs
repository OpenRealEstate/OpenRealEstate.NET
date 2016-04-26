using OpenRealEstate.Core;
using Shouldly;

namespace OpenRealEstate.Tests.Transmorgrifiers.REA
{
    public static class UnitOfMeasureAssertHelpers
    {
        public static void AssertUnitOfMeasure(UnitOfMeasure source, UnitOfMeasure destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            source.Type.ShouldBe(destination.Type);
            source.Value.ShouldBe(destination.Value);
        }
    }
}