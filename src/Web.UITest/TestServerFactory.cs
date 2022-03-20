using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.UITest;

class TestServerFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public string RootUri { get; set; } //Save this use by tests

    private IWebHost _webHost;
    
    private Action<IServiceCollection> _configureTestServices;

    public TestServerFactory()
    {
        ClientOptions.BaseAddress = new Uri("https://localhost");
    }

    public HttpClient Create()
    {
        return this.CreateClient();
    }

    public void ConfigureTestServices(Action<IServiceCollection> configureTestServices)
    {
        this._configureTestServices = configureTestServices;
    }
    
    protected override IWebHostBuilder? CreateWebHostBuilder()
    {
        return WebHost.CreateDefaultBuilder().UseStartup<TStartup>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseUrls(ClientOptions.BaseAddress.ToString());
    }

    protected override TestServer CreateServer(IWebHostBuilder builder)
    {
        if(_configureTestServices is not null)
            builder.ConfigureTestServices(s => _configureTestServices(s));
        
        _webHost = builder.Build();
        _webHost.Start();
        RootUri = _webHost.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault();
        
        return base.CreateServer(WebHost.CreateDefaultBuilder().UseStartup<TStartup>());
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if(_webHost is not null)
            _webHost.Dispose();
    }
}