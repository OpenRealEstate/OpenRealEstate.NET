using System;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Residential;
using Ploeh.AutoFixture;

namespace OpenRealEstate.FakeData.Customizations
{
    public class ResidentialListingCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ResidentialListing>(c =>
                c.With(x => x.CouncilRates, "testing 1 2 3")
                );
            //.Do(l =>
            //    {
            //        var random = new Random();
            //        var randomPropertyType = random.Next(0, Enum.GetValues(typeof(PropertyType)).Length - 1);

            //        l.PropertyType = 
            //            (PropertyType)Enum.GetValues(typeof(PropertyType)).GetValue(randomPropertyType);
            //    }));
            //fixture.Customize<ResidentialListing>(c =>
            //{
            //    var random = new Random();
            //    var randomPropertyType = random.Next(0, Enum.GetValues(typeof(PropertyType)).Length - 1);

            //    c.With(x => x.PropertyType,
            //        (PropertyType) Enum.GetValues(typeof(PropertyType)).GetValue(randomPropertyType));
            //    return c;
            //});
        }
    }
}