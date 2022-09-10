namespace MauiB2C.Services;

public class Authenticator : IAuthenticator
{
    public async Task<LoginResult> AuthenticateAsync(Uri authUri, Uri callbackUri)
    {
        var myUri = new Uri("https://mauib2c.b2clogin.com/mauib2c.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1_susi&client_id=6ae54df0-6ae2-4e59-b57d-31b995328353&nonce=defaultNonce&redirect_uri=auth.mauib2c%3A%2F%2Fauth&scope=openid%20https%3A%2F%2Fmauib2c.onmicrosoft.com%2Fmaui-api%2Faccess_as_user&response_type=id_token%20token&prompt=login");

        Console.WriteLine($"[Supplied URI] {authUri}");
        Console.WriteLine($"[Working URI] {myUri}");


        var result = await WebAuthenticator.AuthenticateAsync(myUri, callbackUri);

        return new LoginResult
        {
            AccessToken     = result.AccessToken,
            IdToken         = result.IdToken,
            RefreshToken    = result.RefreshToken,
        };
    }
}
