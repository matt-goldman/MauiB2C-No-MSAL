using System.IdentityModel.Tokens.Jwt;
using System.Web;

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

    private readonly Uri RedirectUri;

    private readonly Uri AuthUrl;
    private readonly IAuthenticator _authenticator;

    public AuthService(IAuthenticator authenticator, B2COptions options)
    {
        RedirectUri = new Uri(HttpUtility.UrlEncode(options.RedirectUri));

        AuthUrl = new Uri($"https://{options.Domain}.b2clogin.com/{options.Domain}.onmicrosoft.com/oauth2/v2.0/authorize?" +
            $"p={options.Policy}&" +
            $"client_id={options.ClientId}&" +
            $"nonce=defaultNonce&" +
            $"redirct_uri={RedirectUri}&" +
            $"scope={HttpUtility.UrlEncode(options.Scope)}&" +
            $"response_type=id_token token&" +
            $"prompt=login");
        _authenticator = authenticator;
    }

    internal static string AccessToken { get; set; } = String.Empty;

    internal static string RefreshToken { get; set; } = String.Empty;

    internal static string GetToken() => AccessToken;

    public async Task<bool> LoginAsync()
    {
        try
        {
            var loginResult = await _authenticator.AuthenticateAsync(AuthUrl, RedirectUri);

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
        RefreshToken = await SecureStorage.GetAsync(nameof(RefreshToken));

        if (string.IsNullOrEmpty(RefreshToken))
            return false;

        //var result = await oidcClient.RefreshTokenAsync(RefreshToken);

        //if (result.IsError)
        //{
        //    // TODO inspect and handle error
        //    return false;
        //}

        //await SetRefreshToken(result.RefreshToken);
        //SetLoggedInState(result?.AccessToken ?? String.Empty, result?.IdentityToken ?? string.Empty);
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
        await SecureStorage.SetAsync(nameof(RefreshToken), token);
    }

    private void ClearTokens()
    {
        RefreshToken = String.Empty;
        SecureStorage.Remove(nameof(RefreshToken));
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