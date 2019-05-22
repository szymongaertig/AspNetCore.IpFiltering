using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AspNetCore.Whitelist
{
    public class DefaultIpResultCache : IIpAddressResultCache
    {
        private readonly ConcurrentDictionary<IPAddress, (DateTime expirationTime,bool result)> _ipResults;
        // If null, never expire
        private int? _durationTime { get; set; }

        public DefaultIpResultCache(int? durationTime)
        {
            _durationTime = durationTime;
            _ipResults = new ConcurrentDictionary<IPAddress, (DateTime, bool)>();
        }
        
        public Task<bool?> AllowAddress(IPAddress remoteAddress)
        {
            var currentTime = DateTime.Now;
            if (!_ipResults.ContainsKey(remoteAddress))
            {
                return Task.FromResult<bool?>(null);
            }
            var cachedResult = _ipResults[remoteAddress];
            if (cachedResult.expirationTime >= currentTime)
            {
                return Task.FromResult<bool?>(cachedResult.result);
            }

            if (!_ipResults.TryRemove(remoteAddress, out var oldValue))
            {
                throw new InvalidOperationException($"Could not remove item with key: {remoteAddress} to _ipResults dictionary");
            }
            
            return Task.FromResult<bool?>(null);
        }

        public Task SaveResult(IPAddress remoteAddress, bool result)
        {
            if (!_ipResults.ContainsKey(remoteAddress))
            {
                if (_durationTime.GetValueOrDefault() > 0)
                {
                    var newValue = (DateTime.Now.AddSeconds(_durationTime.GetValueOrDefault()), result);

                    if (!_ipResults.TryAdd(remoteAddress, value: newValue))
                    {
                        throw new InvalidOperationException($"Could not add item with key: {remoteAddress} to _ipResults dictionary");
                    }
                    
                }
            }

            return Task.FromResult(0);
        }
    }
}