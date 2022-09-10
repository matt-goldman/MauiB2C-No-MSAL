using MauiB2C.Services;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

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

            builder.Services.AddHttpClient(AuthService.AuthenticatedClient)
                .AddHttpMessageHandler((s) => s.GetService<AuthHandler>());

            builder.Services.AddSingleton<IBrowser, AuthBrowser>();
            builder.Services.AddSingleton<IAuthService, AuthService>();

            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}