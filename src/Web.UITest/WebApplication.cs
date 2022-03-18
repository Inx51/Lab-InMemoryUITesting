using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Web.UITest;

class WebApplication : WebApplicationFactory<Program>
{
    
    private const string _LocalhostBaseAddress = "https://localhost";
    
    private IWebHost  _host;
    
    public string RootUri { get; private set; }
    protected override TestServer CreateServer(IWebHostBuilder builder)
    {
        _host = builder.Build();
        _host.Start();
        RootUri = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault();
        // not used but needed in the CreateServer method logic
        return new TestServer(builder);
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // WebApplication.CreateBuilder(args);

        return base.CreateHost(builder);
    }
}