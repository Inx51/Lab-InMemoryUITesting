using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;

namespace Web.UITest;

[TestClass]
public class UnitTest1
{
    TstrServerFactory<Startup> Server { get; set; }
    public TestContext TestContext { get; set; }

    private IPlaywright Playwright;
    
    [TestInitialize]
    public async Task Initialize()
    {
        var tstrServerFactoryOptions = new TstrServerFactoryOptions
        {
            EnableHttpListener = true
        };
        this.Server = new TstrServerFactory<Startup>(tstrServerFactoryOptions);
        this.Server.Run();
        
        Microsoft.Playwright.Playwright.In
        this.Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        

        // var chromeOptions = new ChromeOptions
        // {
        //     AcceptInsecureCertificates = true
        // };
        // Browser = new ChromeDriver(chromeOptions);
    }
    
    // [TestMethod]
    // public async Task TestMethod1()
    // {
    //     Browser.Navigate().GoToUrl(Server.RootUri+"/products/list");
    //     await Task.Delay(1000);
    //     Browser.TakeScreenshot().SaveAsFile(TestContext.TestDir+"/ss_before_removed_product.png");
    //     
    //     Browser.FindElements(By.CssSelector(".btn")).First().Click();
    //     
    //     Browser.TakeScreenshot().SaveAsFile(TestContext.TestDir+"/ss_after_removed_product.png");
    //     
    //     Browser.Dispose();
    // }
    
    [TestMethod]
    public async Task TestMethod1()
    {
        var browser = await Playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync();
        await page.GotoAsync($"{this.Server.RootUri}/products/list");
    }
}