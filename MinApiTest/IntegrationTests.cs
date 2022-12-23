using Microsoft.AspNetCore.Mvc.Testing;

namespace MinApiTest;

[TestClass]
public class IntegrationTests
{
    private static WebApplicationFactory<Program> app;

    [ClassInitialize]
    public static void Init(TestContext testcontext)
    {
        app = new WebApplicationFactory<Program>();
    }

    [TestMethod]
    public async Task TestHome()
    {
        var client = app.CreateClient();
        var resp = await client.GetStringAsync("/");
        Assert.AreEqual("Hello World from SUT/1.0", resp);
    }

    [TestMethod]
    public async Task TestGuid()
    {
        var client = app.CreateClient();
        var resp = await client.GetStringAsync("/guid");
        Assert.IsTrue(resp.StartsWith("SUT:"));
        Assert.IsTrue(Guid.TryParse(resp.Substring(4), out _));
    }

}