[![Build status](https://ci.appveyor.com/api/projects/status/d2y382vrdtbnkbgj?svg=true)](https://ci.appveyor.com/project/garfieldos/aspnetcore-whitelist)

# AspNetCore.Whitelist

A midleware that allows whitelist or blacklist incoming requests. 

It supports: 
* single IP
* IP range IPv4 and IPv6
* wildcard (*)
* configurable caching

Configuration of whitelist and blacklist addresses can be made by: asp.net Core configuration system or by implementing custom `IIpRulesProvider`.

## Get in on NuGet
```
Install-Package DotNetCore.Whitelist
```

## Usage

## Appsetting based configuration

### Startup.cs file:
```
public class Startup
    {
        // ....

        public void ConfigureServices(IServiceCollection services)
        {
            // ...

            services.AddWhiteList(_configuration.GetSection("Whitelist"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // ...

            app.UseWhitelistMiddleware();
            app.UseMvc();
        }
    }
```

### appsettings.yml file:
```
{
    "Whitelist" : {
        "Whitelist": ["*"],
        "Blacklist": [],
        "IpRulesSource": "Configuration",
        "IpRuleCacheSource" : "Configuration",
        "DefaultIpRuleCacheDuration" : "300",
        "FailureHttpStatusCode": "404"
    },
}
```
## Custom provider based configuration 

### Startup.cs file:
```
public class Startup
    {
        // ....

        public void ConfigureServices(IServiceCollection services)
        {
            // ...
            
            services.AddTransient<IIpRulesProvider, InMemoryListRulesProvider>();
            services.AddWhiteList(new WhitelistOptions
            {
                IpListSource = IpListSource.Provider,
                FailureHttpStatusCode = 404
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // ...

            app.UseWhitelistMiddleware();
            app.UseMvc();
        }
    }

```

### InMemoryListRulesProvider class:

```
    public class InMemoryListRulesProvider : IIpRulesProvider
    {
        public Task<IpRule[]> GetIpRules()
        {
            return Task.FromResult(new List<IpRule>()
            {
                // blacklist
                new IpRule(IpAddressRangeWithWildcard.Parse("127.0.0.4"),
                    IpRuleType.Blacklist),
                new IpRule(IpAddressRangeWithWildcard.Parse("127.0.0.4"),
                    IpRuleType.Blacklist),
                
                // whitelist
                new IpRule(IpAddressRangeWithWildcard.GetWildcardRange(),IpRuleType.Whitelist)
            }.ToArray());
        }
    }
```

In real implementation you would make a real db call instead of returning static list.

## More samples can be found here: 
[https://github.com/garfieldos/AspNetCore.Whitelist/tree/master/src/samples](https://github.com/garfieldos/AspNetCore.Whitelist/tree/master/src/samples)
