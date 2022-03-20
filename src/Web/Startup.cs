using Web;

public class Startup
{
    public WebApplication InitializeApp()
    {
        var app = WebApplication.CreateBuilder();
        ConfigureServices(app.Services);
        var builder = app.Build();
        Configure(builder, app.Environment);
        return builder;
    }
    
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddHttpClient<IProductsHttpClientService, ProductsHttpClientService>()
            .ConfigureHttpClient(config => { config.BaseAddress = new Uri("http://localhost:5232"); });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoint => endpoint.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"));
    }
}
