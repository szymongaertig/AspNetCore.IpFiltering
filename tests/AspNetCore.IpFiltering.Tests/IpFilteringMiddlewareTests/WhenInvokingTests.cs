using System.IO;
using System.Net;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace AspNetCore.IpFiltering.Tests.IpFilteringMiddlewareTests
{
    public class WhenInvokingTests
    {
        [Theory]
        [InlineData("/any/path", 403)]
        public async Task ForNotAllowedIp_ThenReturnProperStatusCode(
            string requestPath,
            int statusCode)
        {
            // Given
            var fixture = new IpFilteringMiddlewareTestsFixture(requestPath);
            fixture.CacheMock.Setup(x => x.AllowAddress(It.IsAny<IPAddress>()))
                .ReturnsAsync(false);

            fixture.AddressValidatorMock.Setup(x => x.AllowAccess(It.IsAny<IPAddress>()))
                .ReturnsAsync(false);

            fixture.OptionsMock.SetupGet(x => x.FailureHttpStatusCode)
                .Returns(statusCode);

            // When
            await fixture.Sut.Invoke(fixture.HttpContext);

            // Then
            Assert.Equal(statusCode, fixture.HttpContext.Response.StatusCode);
        }

        /*[Theory]
        [InlineData("/any/path", "Some failure message")]
        public async Task ForNotAllowedIp_ThenReturnProperFailureMessage(
            string requestPath,
            string failureMessage)
        {
            // Given
            var fixture = new IpFilteringMiddlewareTestsFixture(requestPath);
            fixture.CacheMock.Setup(x => x.AllowAddress(It.IsAny<IPAddress>()))
                .ReturnsAsync(false);

            fixture.AddressValidatorMock.Setup(x => x.AllowAccess(It.IsAny<IPAddress>()))
                .ReturnsAsync(false);

            fixture.OptionsMock.SetupGet(x => x.FailureMessage)
                .Returns(failureMessage);

            // When
            await fixture.Sut.Invoke(fixture.HttpContext);

            // Then           
            string body;
            using (var sr = new StreamReader(fixture.HttpContext.Response.Body))
            {
                body = sr.ReadToEnd();
            }

            Assert.Equal(failureMessage, body);
        } */

        [Theory]
        [InlineData("/any/path")]
        public async Task ForAllowedIp_ThenReturnProperStatusCode(
            string requestPath)
        {
            // Given
            var fixture = new IpFilteringMiddlewareTestsFixture(requestPath);
            fixture.CacheMock.Setup(x => x.AllowAddress(It.IsAny<IPAddress>()))
                .ReturnsAsync(true);

            fixture.AddressValidatorMock.Setup(x => x.AllowAccess(It.IsAny<IPAddress>()))
                .ReturnsAsync(true);

            // When
            await fixture.Sut.Invoke(fixture.HttpContext);

            // Then
            Assert.Equal(200, fixture.HttpContext.Response.StatusCode);
        }
    }
}