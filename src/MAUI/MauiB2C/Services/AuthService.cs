using MauiB2C.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace MauiB2C.Services;

public interface IAuthService
{
    Task<bool> LoginAsync();

    Task<bool> RefreshLoginAsync();

    bool Logout();
}

public class AuthService : IAuthService
{
    public const string AuthenticatedClient = "AuthenticatedClient";
    public const string UnauthenticatedClient = "UnauthenticatedClient";

    private readonly IAuthenticator _authenticator;
    private readonly B2COptions _options;
    private readonly HttpClient _httpClient;

    public AuthService(IAuthenticator authenticator, IHttpClientFactory clientFactory, B2COptions options)
    {
        _authenticator = authenticator;
        _options = options;
        _httpClient = clientFactory.CreateClient(UnauthenticatedClient);
    }

    internal static string AccessToken { get; set; } = String.Empty;

    internal static string RefreshToken { get; set; } = String.Empty;

    internal static string GetToken() => AccessToken;

    public async Task<bool> LoginAsync()
    {
        try
        {
            var loginResult = await _authenticator.AuthenticateAsync(_options);

            await SetRefreshToken(loginResult.RefreshToken);
            SetLoggedInState(loginResult?.AccessToken ?? String.Empty, loginResult?.IdToken ?? string.Empty);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Login failed");
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool Logout()
    {
        try
        {
            ClearTokens();
            AccessToken = String.Empty;
            return true;
        }
        catch (Exception ex)
        {
            // TODO: handle exception
            return false;
        }
    }

    public async Task<bool> RefreshLoginAsync()
    {
        // TODO: Remove this workaround when this issue is resolved: https://github.com/dotnet/maui/issues/8326
#if MACCATALYST
        RefreshToken = Preferences.Get(nameof(RefreshToken), string.Empty);
#else
        RefreshToken = await SecureStorage.GetAsync(nameof(RefreshToken));
#endif

        if (string.IsNullOrEmpty(RefreshToken))
            return false;

        var refreshUri = B2CHelpers.GenerateRefreshUri(_options, RefreshToken);

        var result = await _httpClient.GetFromJsonAsync<LoginResult>(refreshUri);

        await SetRefreshToken(result.RefreshToken);
        SetLoggedInState(result?.AccessToken ?? String.Empty, result?.IdToken ?? string.Empty);
        return true;
    }

    private void SetLoggedInState(string token, string idToken)
    {
        AccessToken = token;

        var claims = ParseToken(idToken);

        var userName = claims?.Claims?.FirstOrDefault(c => c.Type == "name")?.Value;

        if (userName is not null)
        {
            MessagingCenter.Send<object, string>(this, "UsernameUpdated", userName);
        }
    }

    private async Task SetRefreshToken(string token)
    {
        RefreshToken = token;

        // TODO: Remove this workaround when this issue is resolved: https://github.com/dotnet/maui/issues/8326
#if MACCATALYST
        Preferences.Set(nameof(RefreshToken), RefreshToken);
#else
        await SecureStorage.SetAsync(nameof(RefreshToken), token);
#endif
    }

    private void ClearTokens()
    {
        RefreshToken = String.Empty;
        // TODO: Remove this workaround when this issue is resolved: https://github.com/dotnet/maui/issues/8326
#if MACCATALYST
        Preferences.Remove(nameof(RefreshToken));
#else
        SecureStorage.Remove(nameof(RefreshToken));
#endif
    }

    private JwtSecurityToken? ParseToken(string inTtoken)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(inTtoken);

            return token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
}