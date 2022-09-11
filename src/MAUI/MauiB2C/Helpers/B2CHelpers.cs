using MauiB2C.Services;

namespace MauiB2C.Helpers;

public static class B2CHelpers
{
    public static Uri GenerateCodeUri(B2COptions options)
    {
        return new Uri($"https://{options.Domain}.b2clogin.com/{options.Domain}.onmicrosoft.com/oauth2/v2.0/authorize?" +
            $"p={options.Policy}&" +
            $"client_id={options.ClientId}&" +
            $"nonce=defaultNonce&" +
            $"redirect_uri={options.RedirectUri}&" +
            $"scope={options.Scope}&" +
            $"response_type=code&" +
            $"prompt=login");
    }

    public static Uri GenerateTokenUri(B2COptions options, string code)
    {
        return new Uri($"https://{options.Domain}.b2clogin.com/{options.Domain}.onmicrosoft.com/oauth2/v2.0/token?" +
            $"p={options.Policy}&" +
            $"client_id={options.ClientId}&" +
            $"nonce=defaultNonce&" +
            $"redirect_uri={options.RedirectUri}&" +
            $"scope={options.Scope}&" +
            $"grant_type=authorization_code&" +
            $"code={code}");
    }

    public static Uri GenerateRefreshUri(B2COptions options, string refreshToken)
    {
        return new Uri($"https://{options.Domain}.b2clogin.com/{options.Domain}.onmicrosoft.com/oauth2/v2.0/token?" +
            $"p={options.Policy}&" +
            $"client_id={options.ClientId}&" +
            $"nonce=defaultNonce&" +
            $"redirect_uri={options.RedirectUri}&" +
            $"scope={options.Scope}&" +
            $"grant_type=refresh_token&" +
            $"refresh_token={refreshToken}");
    }
}
