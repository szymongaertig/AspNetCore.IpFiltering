using AspNetCore.Whitelist;
using Microsoft.Extensions.Logging;
using Moq;

namespace DotNetCore.Whitelist.Tests.AddressValidatorTests
{
    public class AddressValidatorTestsFixture
    {
        public AddressValidator Sut { get; set; }
        public Mock<IWhitelistIpAddressesProvider> AddressesProviderMock { get; set; }
        public AddressValidatorTestsFixture()
        {
            AddressesProviderMock = new Mock<IWhitelistIpAddressesProvider>();
            var loggerMock = new Mock<ILogger<AddressValidator>>().Object;
            Sut = new AddressValidator(AddressesProviderMock.Object,loggerMock);
        }
    }

}