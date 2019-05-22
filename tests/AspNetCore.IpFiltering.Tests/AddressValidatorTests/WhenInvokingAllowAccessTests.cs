using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace AspNetCore.IpFiltering.Tests.AddressValidatorTests
{
    public class WhenInvokingAllowAccessTests
    {
        private string[] GetListFromStr(string addressesStr)
        {
            return addressesStr.Split(",");
        }

        private List<IpRule> GetAddressDefinitions(
            string whitelist,
            string blacklist)
        {
            var result = new List<IpRule>();

            if (whitelist != null)
            {
                result.AddRange(GetListFromStr(whitelist)
                    .Select(x => x.ConvertToIpRange())
                    .Select(x => new IpRule(x, IpRuleType.Whitelist)));    
            }
            
            if (blacklist != null)
            {
                result.AddRange(GetListFromStr(blacklist)
                    .Select(x => x.ConvertToIpRange())
                    .Select(x => new IpRule(x, IpRuleType.Blacklist)));
            }
            
            return result;
        }

        [Theory]
        [InlineData("127.0.0.1", "127.0.0.1")]
        [InlineData("127.0.0.2,127.0.0.1", "127.0.0.1")]
        public async Task WithRemoteIpExistedInWhitelist(
            string whitelist,
            string remoteIp)
        {
            // Given
            var fixture = new AddressValidatorTestsFixture();
            fixture.AddressesProviderMock.Setup(x => x.GetIpRules())
                .ReturnsAsync(GetAddressDefinitions(whitelist,null).ToArray());

            // When
            var result = await fixture.Sut.AllowAccess(IPAddress.Parse(remoteIp));

            // WHen
            Assert.True(result);
        }

        [Theory]
        [InlineData("*", "127.0.0.1")]
        [InlineData("*,127.0.0.3", "127.0.0.2")]
        [InlineData("*,127.0.0.1,127.0.0.3", "127.0.0.2")]
        public async Task WithWildCardOnWhitelist(string whitelist,
            string remoteIp)
        {
            // Given
            var fixture = new AddressValidatorTestsFixture();
            fixture.AddressesProviderMock.Setup(x => x.GetIpRules())
                .ReturnsAsync(GetAddressDefinitions(whitelist,null).ToArray());

            // When
            var result = await fixture.Sut.AllowAccess(IPAddress.Parse(remoteIp));

            // WHen
            Assert.True(result);
        }

        [Theory]
        [InlineData("127.0.0.1", "127.0.0.1")]
        [InlineData("127.0.0.2,127.0.0.1", "127.0.0.1")]
        public async Task WithRemoteIpExistedOnBlacklist(
            string blackList,
            string remoteIp)
        {
            // Given
            var fixture = new AddressValidatorTestsFixture();fixture.AddressesProviderMock.Setup(x => x.GetIpRules())
                .ReturnsAsync(GetAddressDefinitions(null,blackList).ToArray());

            // When
            var result = await fixture.Sut.AllowAccess(IPAddress.Parse(remoteIp));

            // WHen
            Assert.False(result);
        }

        [Theory]
        [InlineData("127.0.0.2", "127.0.0.5", "127.0.0.1")]
        [InlineData("127.0.0.2,127.0.0.3", "127.0.0.5,127.0.0.4", "127.0.0.1")]
        public async Task WithRemoteIpNotExistingOnAnyList(
            string whitelist,
            string blacklist,
            string remoteIp)
        {
            // Given
            var fixture = new AddressValidatorTestsFixture();
            fixture.AddressesProviderMock.Setup(x => x.GetIpRules())
                .ReturnsAsync(GetAddressDefinitions(whitelist,blacklist).ToArray());

            // When
            var result = await fixture.Sut.AllowAccess(IPAddress.Parse(remoteIp));

            // WHen
            Assert.False(result);
        }

        [Theory]
        [InlineData("127.0.0.1", "127.0.0.1", "127.0.0.1")]
        [InlineData("127.0.0.2,127.0.0.1", "127.0.0.5,127.0.0.1", "127.0.0.1")]
        public async Task WithRemoteIpExistedOnBothLists(
            string whitelist,
            string blacklist,
            string remoteIp)
        {
            // Given
            var fixture = new AddressValidatorTestsFixture();
            fixture.AddressesProviderMock.Setup(x => x.GetIpRules())
                .ReturnsAsync(GetAddressDefinitions(whitelist,blacklist).ToArray());

            // When
            var result = await fixture.Sut.AllowAccess(IPAddress.Parse(remoteIp));

            // WHen
            Assert.False(result);
        }
    }
}