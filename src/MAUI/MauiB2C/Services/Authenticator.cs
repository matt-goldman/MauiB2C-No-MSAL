using MauiB2C.Helpers;
using System.Net.Http.Json;

namespace MauiB2C.Services;

public class Authenticator : IAuthenticator
{
    private readonly HttpClient _httpClient;

    public Authenticator(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient(AuthService.UnauthenticatedClient);
    }

    public async Task<LoginResult> AuthenticateAsync(B2COptions options)
    {
        var codeUri = B2CHelpers.GenerateCodeUri(options);
#if WINDOWS
        var result = await WinUIEx.WebAuthenticator.AuthenticateAsync(codeUri, new Uri(options.RedirectUri));
#else
        var result = await WebAuthenticator.AuthenticateAsync(codeUri, new Uri(options.RedirectUri));
#endif

        var code = result.Properties["code"];

        var tokenUri = B2CHelpers.GenerateTokenUri(options, code);

        var loginResult = await _httpClient.GetFromJsonAsync<LoginResult>(tokenUri);
        return loginResult;
    }
}
