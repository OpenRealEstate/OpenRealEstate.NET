using System;
using OpenRealEstate.Core;
using Ploeh.AutoFixture;

namespace OpenRealEstate.FakeData.Customizations
{
    public class CommunicationCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Communication>(c =>
                {
                    var random = new Random();
                    var randomCommunicationType = random.Next(1, Enum.GetValues(typeof(CommunicationType)).Length - 1);

                    c.With(x => x.CommunicationType,
                        (CommunicationType) Enum.GetValues(typeof(CommunicationType)).GetValue(randomCommunicationType));

                    return c;
                }
            );
        }
    }
}