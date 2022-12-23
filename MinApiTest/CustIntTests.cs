using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MinApiTest;

[TestClass]
public class CustIntTests
{
    public class CustWebAppFactory<TProgram> :
        WebApplicationFactory<TProgram> where TProgram : class
    {
        override protected void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // �����즳���U�A�ȡA�����ۭq����
                var svcDesc = services.FirstOrDefault(s => s.ServiceType == typeof(GuidService));
                if (svcDesc != null) services.Remove(svcDesc);
                services.AddSingleton<GuidService>((services) => new GuidService("TEST"));

                // ���]�w��
                svcDesc = services.First(s => s.ServiceType == typeof(IConfiguration));
                services.Remove(svcDesc);
                // Ū����]�w�ɡA�íק� Code
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddInMemoryCollection(new Dictionary<string, string>() {
                        ["CodeName"] = "TEST"
                    })
                    .Build();
                services.AddSingleton<IConfiguration>((services) => config);
            });
        }
    }

    private static CustWebAppFactory<Program> app;

    [ClassInitialize]
    public static void Init(TestContext testContext)
    {
        app = new CustWebAppFactory<Program>();
    }

    [TestMethod]
    public async Task TestHome()
    {
        var client = app.CreateClient();
        var resp = await client.GetStringAsync("/");
        Assert.AreEqual("Hello World from TEST/1.0", resp);
    }

    [TestMethod]
    public async Task TestGuid()
    {
        var client = app.CreateClient();
        var resp = await client.GetStringAsync("/guid");
        Assert.IsTrue(resp.StartsWith("TEST:"));
        Assert.IsTrue(Guid.TryParse(resp.Substring(5), out _));
    }

}