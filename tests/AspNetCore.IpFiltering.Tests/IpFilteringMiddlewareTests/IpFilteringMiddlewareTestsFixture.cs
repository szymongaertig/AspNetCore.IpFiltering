using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace AspNetCore.IpFiltering.Tests.IpFilteringMiddlewareTests
{
    public class IpFilteringMiddlewareTestsFixture
    {
        public Mock<IDelegateMock> RequestDelegateMock { get; set; }
        public HttpContext HttpContext { get; set; }
        public IpFilteringMiddleware Sut { get; set; }
        public Mock<ILogger<IpFilteringMiddleware>> LoggerMock { get; set; }
        public Mock<IIpAddressValidator> AddressValidatorMock { get; set; }
        public Mock<IIpFilteringOptions> OptionsMock { get; set; }
        public Mock<IIpAddressResultCache> CacheMock { get;set; }

        public IpFilteringMiddlewareTestsFixture(string path)
        {
            RequestDelegateMock = new Mock<IDelegateMock>(); 
            
            RequestDelegateMock
                .Setup(x => x.RequestDelegate(It.IsAny<HttpContext>()))
                .Returns(Task.FromResult(0));
            AddressValidatorMock = new Mock<IIpAddressValidator>();
            OptionsMock = new Mock<IIpFilteringOptions>();
            CacheMock = new Mock<IIpAddressResultCache>();

            LoggerMock = new Mock<ILogger<IpFilteringMiddleware>>();
            Sut = new IpFilteringMiddleware(RequestDelegateMock.Object.RequestDelegate, LoggerMock.Object, 
                AddressValidatorMock.Object, OptionsMock.Object, CacheMock.Object);
            
            HttpContext = new DefaultHttpContext();
            HttpContext.Request.Path = path;
            HttpContext.Response.Body = new MemoryStream();   
        }
    }

    public interface IDelegateMock
    {
        Task RequestDelegate(HttpContext context);
    }
}