using OpenRealEstate.Services;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class TimeZoneInfoHelperTests
    {
        [Theory]
        [InlineData("vic")]
        [InlineData("victoria")]
        [InlineData("new south wales")]
        [InlineData("nsw")]
        [InlineData("act")]
        [InlineData("australian capital territory")]
        [InlineData("qld")]
        [InlineData("queensland")]
        [InlineData("tas")]
        [InlineData("tasmania")]
        [InlineData("nt")]
        [InlineData("northern territory")]
        [InlineData("wa")]
        [InlineData("western australia")]
        [InlineData("sa")]
        [InlineData("south australia")]
        public void GivenAState_ConvertByState_ReturnsATimeZoneInfo(string state)
        {
            // Arrange.

            // Act.
            var timeZoneInfo = TimeZoneInfoHelper.ConvertByState(state);

            // Asert.
            timeZoneInfo.ShouldNotBeNull();
        }
    }
}