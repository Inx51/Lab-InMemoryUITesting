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

public class TstrServerFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public string? RootUri { get; private set; }
    public HttpClient? HttpClient { get; private set; }
    

    private IWebHost? _webHost;
    private readonly TstrServerFactoryOptions _options;
    
    public TstrServerFactory(TstrServerFactoryOptions? options = null)
    {
        if (options is null)
            options = new TstrServerFactoryOptions();
        this._options = options;

        var baseAddress = $"{this._options.Scheme.ToString().ToLower()}://{this._options.Host}";
        if (this._options.Port != 80 || this._options.Port != 443)
            baseAddress += $":{this._options.Port}";

        this.ClientOptions.BaseAddress = new Uri(baseAddress);
        this.ClientOptions.HandleCookies = this._options.HandleCookies;
        this.ClientOptions.AllowAutoRedirect = this._options.AllowAutoRedirect;
        this.ClientOptions.MaxAutomaticRedirections = this._options.MaxAutomaticRedirections;
    }
    
    public void Run()
    {
        this.HttpClient = this.CreateClient();
    }

    protected override IWebHostBuilder? CreateWebHostBuilder()
    {
        if (!this._options.EnableHttpListener)
            return base.CreateWebHostBuilder();
        
        return WebHost.CreateDefaultBuilder().UseStartup<TStartup>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseUrls(this.ClientOptions.BaseAddress.ToString());
    }

    protected override TestServer CreateServer(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices
        (
            s => this._options.ConfigureTestServices?.Invoke(s)
        );

        if (!this._options.EnableHttpListener) 
            return base.CreateServer(builder);
        
        this._webHost = builder.Build();
        this._webHost.Start();
        this.RootUri = _webHost.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses.LastOrDefault();

        return base.CreateServer(WebHost.CreateDefaultBuilder().UseStartup<TStartup>());

    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if(this._options.EnableHttpListener)
            this._webHost?.Dispose();
    }
}