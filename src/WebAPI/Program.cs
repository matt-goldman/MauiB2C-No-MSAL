using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#if DEBUG
var tunnelUrl = Environment.GetEnvironmentVariable("VS_TUNNEL_URL");

Console.WriteLine($"Tunnel URL: {tunnelUrl}");

var Constants = new
{
    BaseUrl = tunnelUrl
};

var mobileConfig = new
{
    Constants = Constants
};

string appDataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mauib2c");
Directory.CreateDirectory(appDataFolderPath);

string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
string filePath = Path.Combine(appDataPath, ".mauib2c", "appsettings.debug.json");

var jsonConfig = JsonSerializer.Serialize(mobileConfig);

File.WriteAllText(filePath, jsonConfig);
#endif

app.Run();
