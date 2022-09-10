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

            builder.Services.AddHttpClient(AuthService.AuthenticatedClient)
                .AddHttpMessageHandler((s) => s.GetService<AuthHandler>());

            var options = new B2COptions
            {
                Domain      = "mauib2c",
                ClientId    = "6ae54df0-6ae2-4e59-b57d-31b995328353",
                Policy      = "b2c_1_susi",
                Scope       = "openid https://mauib2c.onmicrosoft.com/maui-api/access_as_user",
                RedirectUri = "auth.mauib2c://auth"
            };

            builder.Services.AddSingleton(options);

#if WINDOWS
            builder.Services.AddSingleton<IAuthenticator, WindowsAuthenticator>();
            Console.WriteLine("[MauiProgram] Windows platform initialised");
#else
            builder.Services.AddSingleton<IAuthenticator, Authenticator>();
#endif

            builder.Services.AddSingleton<IAuthService, AuthService>();

            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}