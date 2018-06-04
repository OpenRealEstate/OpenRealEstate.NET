using OpenRealEstate.Core.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class MediaTests
    {
        [Fact]
        public void MediaShouldSortByValueOfOrderProperty()
        {
            var media1 = new Media { Order = 1 };
            var media2 = new Media { Order = 2 };
            var media3 = new Media { Order = 3 };

            //purposely put these in jumbled order
            var mediaList = new List<Media>
            {
                media2,
                media3,
                media1
            };

            //Assert jumbed order
            mediaList[0].Order.ShouldBe(2);
            mediaList[1].Order.ShouldBe(3);
            mediaList[2].Order.ShouldBe(1);

            //Act
            mediaList.Sort();

            //Assert
            mediaList[0].Order.ShouldBe(1);
            mediaList[1].Order.ShouldBe(2);
            mediaList[2].Order.ShouldBe(3);

        }
    }
}
