using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Web.UITest;

[TestClass]
public class UnitTest1
{
    [TestInitialize]
    public void Initialize()
    {
        var webApplication = new WebApplication();
        var httpClient = webApplication.CreateClient();
    }
    
    [TestMethod]
    public void TestMethod1()
    {
    }
}