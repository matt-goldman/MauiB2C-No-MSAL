using MauiB2C.Helpers;
using System.Net.Http.Json;

namespace MauiB2C.Services;

public class Authenticator : IAuthenticator
{
    public async Task<LoginResult> AuthenticateAsync(B2COptions options)
    {
        var codeUri = B2CHelpers.GenerateCodeUri(options);

        var result = await WebAuthenticator.AuthenticateAsync(codeUri, new Uri(options.RedirectUri));

        var code = result.Properties["code"];

        var tokenUri = B2CHelpers.GenerateTokenUri(options, code);

        using (var http = new HttpClient())
        {
            var loginResult = await http.GetFromJsonAsync<LoginResult>(tokenUri);
            return loginResult;
        }
    }
}
