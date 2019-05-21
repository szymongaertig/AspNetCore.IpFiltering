[![Build status](https://ci.appveyor.com/api/projects/status/d2y382vrdtbnkbgj?svg=true)](https://ci.appveyor.com/project/garfieldos/aspnetcore-whitelist)

# AspNetCore.Whitelist

A midleware that allows whitelist or blacklist incoming requests. 

It supports: 
* single IP
* IP range IPv4 and IPv6
* wildcard (*)

Configuration of whitelist and blacklist addresses can be made by: asp.net Core configuration system or by implementing custom `IWhitelistIpAddressesProvider`.

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
    "Whitelist": ["127.0.0.1"],
    "Blacklist": [],
    "IpListSource": "Configuration",
    "FailureHttpStatusCode": "404"
  }
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
            
            services.AddTransient<IWhitelistIpAddressesProvider, InMemoryListAddressesProvider>();
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

### InMemoryListAddressesProvider class:

```
    public class InMemoryListAddressesProvider : IWhitelistIpAddressesProvider
    {
        public Task<IpAddressRangeWithWildcard[]> GetBlacklist()
        {
            return Task.FromResult(new[] {
                IpAddressRangeWithWildcard.Parse("127.0.0.4"),
                IpAddressRangeWithWildcard.Parse("127.0.0.5")
            });
        }

        public Task<IpAddressRangeWithWildcard[]> GetWhitelist()
        {
            return Task.FromResult(new[] {
                IpAddressRangeWithWildcard.GetWildcardRange()
            });
        }
    }
```

In real implementation you would make a real db call instead of returning static list.

## More samples can be found here: 
[https://github.com/garfieldos/AspNetCore.Whitelist/tree/master/src/samples](https://github.com/garfieldos/AspNetCore.Whitelist/tree/master/src/samples)
