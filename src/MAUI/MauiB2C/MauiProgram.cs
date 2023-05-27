using MauiB2C.Services;

namespace MauiB2C
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<AuthHandler>();

            builder.Services.AddHttpClient(AuthService.AuthenticatedClient, (opt) => 
                        opt.BaseAddress = new Uri(Constants.BaseUrl))
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