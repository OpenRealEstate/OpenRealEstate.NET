using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Residential;
using Ploeh.AutoFixture;

namespace OpenRealEstate.FakeData
{
    public class ResidentialListingCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ResidentialListing>(x => x.With(y => y.PropertyType, PropertyType.House));
        }
    }
}
