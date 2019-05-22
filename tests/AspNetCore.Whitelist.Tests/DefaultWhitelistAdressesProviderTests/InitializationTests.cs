using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.Whitelist.Tests.DefaultWhitelistAdressesProviderTests
{
    public class InitializationTests
    {
        private string[] GetListFromStr(string addressesStr)
        {
            return addressesStr.Split(",");
        }

        [Fact]
        public async Task WithoutBlacklistAndWhitelistProvided_ThenReturnValidNUmberOfElements()
        {
            // When
            var provider = new DefaultIpRulesProvider(null,null);
            
            // Then
            Assert.NotNull(provider);
            var numberOfIpRules = await provider.GetIpRules();

            var numberOfWhitelistElements = numberOfIpRules.Where(x => x.Type == IpRuleType.Whitelist);
            Assert.Empty(numberOfWhitelistElements);
            
            var numberOfBlacklistElements = numberOfIpRules.Where(x => x.Type == IpRuleType.Blacklist);
            Assert.Empty(numberOfBlacklistElements);
        }
        
        [Theory]
        [InlineData("*,127.0.0.1",2)]
        [InlineData("127.0.0.1,127.0.0.1",1)]
        [InlineData("127.0.0.1-127.0.0.3,127.0.0.5-127.0.0.8",2)]
        public async Task WithWhitelistProvided_ThenReturnsValidWhitelistElements(string whiteList,
            int numberOfElements)
        {
            // Given
            var whitelistStr = GetListFromStr(whiteList);
            
            // When
            var provider = new DefaultIpRulesProvider(whitelistStr,new string[0]);
            
            // Then
            Assert.NotNull(provider);
            var numberOfWhiteListElements = (await provider.GetIpRules()).Where(x => x.Type == IpRuleType.Whitelist);
            Assert.Equal(numberOfElements, numberOfWhiteListElements.Count());
        }
        
        
        [Theory]
        [InlineData("*,127.0.0.1",2)]
        [InlineData("127.0.0.1,127.0.0.1",1)]
        [InlineData("127.0.0.1-127.0.0.3,127.0.0.5-127.0.0.8",2)]
        public async Task WithBlacklistProvided_ThenReturnsValidWhitelistElements(string blacklist,
            int numberOfElements)
        {
            // Given
            var blacklistStr = GetListFromStr(blacklist);
            
            // When
            var provider = new DefaultIpRulesProvider(null,blacklistStr);
            
            // Then
            Assert.NotNull(provider);
            var numberOfBlacklistElements = (await provider.GetIpRules()).Where(x => x.Type == IpRuleType.Blacklist);
            Assert.Equal(numberOfElements, numberOfBlacklistElements.Count());
        }
    }
}