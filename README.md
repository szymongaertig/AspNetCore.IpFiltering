[![Build status](https://ci.appveyor.com/api/projects/status/0i1q8dxpl876jfkj?svg=true)](https://ci.appveyor.com/project/garfieldos/aspnetcore-whitelist)

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

## Appsetting base configuration

Startup.cs file:
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

appsettings.yml file:
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

## More samples can be found here: 
[https://github.com/garfieldos/AspNetCore.Whitelist/tree/master/src/samples](https://github.com/garfieldos/AspNetCore.Whitelist/tree/master/src/samples)
