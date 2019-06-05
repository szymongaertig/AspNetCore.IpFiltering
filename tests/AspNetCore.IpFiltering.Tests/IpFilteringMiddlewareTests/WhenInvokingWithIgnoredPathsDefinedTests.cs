using System.Net;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace AspNetCore.IpFiltering.Tests.IpFilteringMiddlewareTests
{
    public class WhenInvokingWithIgnoredPathsDefinedTests
    {
        [Theory]
        [InlineData("/api/path1", "/api/path1")]
        [InlineData("/api/some-data/[\\d]{1,}", "/api/some-data/11")]
        [InlineData("/api/some-data/", "/api/some-data/11")]
        public async Task ForMatchingPath_ThenDoesNotInvokeCacheMethods(
            string ignoredPath,
            string requestPath)
        {
            // Given
            var fixture = new IpFilteringMiddlewareTestsFixture(requestPath);

            fixture.OptionsMock.SetupGet(x => x.IgnoredPaths)
                .Returns(new[] {ignoredPath});

            // When
            await fixture.Sut.Invoke(fixture.HttpContext);

            // Then
            fixture.CacheMock.Verify(x => x.AllowAddress(It.IsAny<IPAddress>()),
                Times.Never);
        }

        [Theory]
        [InlineData("/api/path2", "/api/path1")]
        [InlineData("/api/some-data/[\\d]{1,}", "/api/some-data/some-other-property/1")]
        [InlineData("/api/some-data1/", "/api/some-data/11")]
        public async Task ForNotMatchingPath_ThenInvokesCacheMethods(
            string ignoredPath,
            string requestPath)
        {
            // Given
            var fixture = new IpFilteringMiddlewareTestsFixture(requestPath);

            fixture.OptionsMock.SetupGet(x => x.IgnoredPaths)
                .Returns(new[] {ignoredPath});

            fixture.CacheMock.Setup(x => x.AllowAddress(It.IsAny<IPAddress>()))
                .ReturnsAsync(true);

            // When
            await fixture.Sut.Invoke(fixture.HttpContext);

            // Then
            fixture.CacheMock.Verify(x => x.AllowAddress(It.IsAny<IPAddress>()),
                Times.Once);
        }
    }
}