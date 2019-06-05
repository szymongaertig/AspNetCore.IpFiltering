using System.Net;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace AspNetCore.IpFiltering.Tests.IpFilteringMiddlewareTests
{
    public class WhenInvokingWithLearningModeTests
    {        
        [Theory]
        [InlineData("/any/path")]
        public async Task ForNotAllowedIp_AndLearningModeTurnedOn_ThenReturnHttp200StatusCode(
            string requestPath)
        {
            // Given
            var fixture = new IpFilteringMiddlewareTestsFixture(requestPath);
            fixture.CacheMock.Setup(x => x.AllowAddress(It.IsAny<IPAddress>()))
                .ReturnsAsync(false);

            fixture.AddressValidatorMock.Setup(x => x.AllowAccess(It.IsAny<IPAddress>()))
                .ReturnsAsync(false);

            fixture.OptionsMock.SetupGet(x => x.LearningMode)
                .Returns(true);
            
            // When
            await fixture.Sut.Invoke(fixture.HttpContext);
            
            // Then
            Assert.Equal(200,fixture.HttpContext.Response.StatusCode);
        }
        
        [Theory]
        [InlineData("/any/path",403)]
        public async Task ForNotAllowedIp_AndLearningModeTurnedOff_ThenReturnProperStatusCode(
            string requestPath,
            int failedStatusCode)
        {
            // Given
            var fixture = new IpFilteringMiddlewareTestsFixture(requestPath);
            fixture.CacheMock.Setup(x => x.AllowAddress(It.IsAny<IPAddress>()))
                .ReturnsAsync(false);

            fixture.AddressValidatorMock.Setup(x => x.AllowAccess(It.IsAny<IPAddress>()))
                .ReturnsAsync(false);

            fixture.OptionsMock.SetupGet(x => x.FailureHttpStatusCode)
                .Returns(failedStatusCode);
            
            fixture.OptionsMock.SetupGet(x => x.LearningMode)
                .Returns(false);
            
            // When
            await fixture.Sut.Invoke(fixture.HttpContext);
            
            // Then
            Assert.Equal(failedStatusCode,fixture.HttpContext.Response.StatusCode);
        }
    }
}