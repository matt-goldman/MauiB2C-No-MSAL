using MauiB2C.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace MauiB2C
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Add this config to the builder
            var a = typeof(MauiProgram).Assembly;
#if DEBUG
            using var stream = a.GetManifestResourceStream("appsettings.debug.json");
#else
            using var stream = a.GetManifestResourceStream("appsettings.json");
#endif
            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();
            
            builder.Configuration.AddConfiguration(config);
            
            // back to normal here
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<AuthHandler>();

            // now add this
            var constants = config.GetSection(nameof(Constants)).Get<Constants>();

            builder.Services.AddHttpClient(AuthService.AuthenticatedClient, (opt) => 
                        opt.BaseAddress = new Uri(constants.BaseUrl)) // and change this
                .AddHttpMessageHandler((s) => s.GetService<AuthHandler>());

            builder.Services.AddHttpClient(AuthService.UnauthenticatedClient);

            // Replace these with the values from your B2C tenant
            // Don't forget to update AppSettings.json in your API too
            var options = new B2COptions
            {
                Domain      = "mauib2c",
                ClientId    = "6ae54df0-6ae2-4e59-b57d-31b995328353",
                Policy      = "b2c_1_susi",
                Scope       = $"openid offline_access https://mauib2c.onmicrosoft.com/maui-api/access_as_user",
                RedirectUri = "auth.mauib2c://auth"
            };

            builder.Services.AddSingleton(options);

            builder.Services.AddSingleton<IAuthenticator, Authenticator>();

            builder.Services.AddSingleton<IAuthService, AuthService>();

            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}