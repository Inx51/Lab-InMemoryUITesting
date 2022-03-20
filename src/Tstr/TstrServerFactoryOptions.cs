using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Web.UITest;

public class TstrServerFactoryOptions
{
    public int Port { get; set; } = 80;
    public HttpScheme Scheme { get; set; } = HttpScheme.Http;
    public string Host { get; set; } = "localhost";
    public bool HandleCookies { get; set; } = true;
    public bool AllowAutoRedirect { get; set; } = true;
    public int MaxAutomaticRedirections { get; set; } = 7;
    public bool EnableHttpListener { get; set; } = false;
    public Action<IServiceCollection> ConfigureTestServices { get; set; }
}