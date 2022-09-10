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
        var result = await WinUIEx.WebAuthenticator.AuthenticateAsync(authUri, callbackUri);
        return new LoginResult
        {
            AccessToken     = result.AccessToken,
            IdToken         = result.IdToken,
            RefreshToken    = result.RefreshToken, 
        };
    }
}
#endif