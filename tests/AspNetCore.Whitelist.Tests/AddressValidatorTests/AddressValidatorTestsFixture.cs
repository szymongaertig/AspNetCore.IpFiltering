using AspNetCore.Whitelist;
using Microsoft.Extensions.Logging;
using Moq;

namespace DotNetCore.Whitelist.Tests.AddressValidatorTests
{
    public class AddressValidatorTestsFixture
    {
        public AddressValidator Sut { get; set; }
        public Mock<IIpRulesProvider> AddressesProviderMock { get; set; }
        public AddressValidatorTestsFixture()
        {
            AddressesProviderMock = new Mock<IIpRulesProvider>();
            var loggerMock = new Mock<ILogger<AddressValidator>>().Object;
            Sut = new AddressValidator(AddressesProviderMock.Object,loggerMock);
        }
    }

}