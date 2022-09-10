namespace MauiB2C.Services;

public interface IAuthenticator
{
    Task<LoginResult> AuthenticateAsync(Uri authUri, Uri callbackUri);
}

public class LoginResult
{
    public string IdToken { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}
