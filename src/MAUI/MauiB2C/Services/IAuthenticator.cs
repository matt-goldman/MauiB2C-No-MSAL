using System.Text.Json.Serialization;

namespace MauiB2C.Services;

public interface IAuthenticator
{
    Task<LoginResult> AuthenticateAsync(B2COptions options);
}

public class LoginResult
{
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}
