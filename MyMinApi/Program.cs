var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<GuidService>();
    
var app = builder.Build();

app.MapGet("/", (IConfiguration config) =>
    $"Hello World from {config["CodeName"]}/{config["Version"]}");
app.MapGet("/guid", (GuidService guidService) => guidService.GetGuid());

app.Run();

public partial class Program { }
public class GuidService
{
    private string _src;
    public GuidService(string src = "SUT")
    {
        _src = src;
    }
    public string GetGuid() => $"{_src}:{Guid.NewGuid()}";
}
