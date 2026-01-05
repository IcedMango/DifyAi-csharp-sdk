using DifyAi.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ============================================================
// Register Dify AI services
// ============================================================
builder.Services.AddDifyAi(register =>
{
    // Method 1: Quick registration (recommended)
    // Read API Key from configuration file
    var config = builder.Configuration;

    register.RegisterBot(
        name: "MyBot",
        apiKey: config["DifyAi:BotApiKey"] ?? "app-xxxxxxxx",
        baseUrl: config["DifyAi:BaseUrl"] ?? "https://api.dify.ai/v1/"
    );

    register.RegisterDataset(
        name: "MyDataset",
        apiKey: config["DifyAi:DatasetApiKey"] ?? "dataset-xxxxxxxx",
        baseUrl: config["DifyAi:BaseUrl"] ?? "https://api.dify.ai/v1/"
    );

    // Method 2: Advanced registration with proxy and SSL settings
    // register.RegisterBot(new DifyAiInstanceConfig
    // {
    //     Name = "ProxyBot",
    //     ApiKey = "app-xxxxxxxx",
    //     BaseUrl = "https://api.dify.ai/v1/",
    //     ProxyUrl = "socks5://127.0.0.1:8889",
    //     IgnoreSslErrors = true  // For development/testing only
    // });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
