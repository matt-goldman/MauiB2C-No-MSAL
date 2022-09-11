#if WINDOWS
namespace MauiB2C.Services;

public class WindowsAuthenticator : IAuthenticator
{
    public WindowsAuthenticator()
	{
        Console.WriteLine("[WindowsAuthenticator] Windows Authenticator constructed");
	}

    public async Task<LoginResult> AuthenticateAsync(Uri authUri, Uri callbackUri)
    {
        var codeUri = B2CHelpers.GenerateCodeUri(options);

        var result = await WinUIEx.WebAuthenticator.AuthenticateAsync(authUri, callbackUri);

        var code = result.Properties["code"];

        var tokenUri = B2CHelpers.GenerateTokenUri(options, code);

        var loginResult = await _httpClient.GetFromJsonAsync<LoginResult>(tokenUri);
        return loginResult;
    }
}
#endif