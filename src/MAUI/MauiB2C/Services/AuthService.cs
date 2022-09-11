﻿using System.IdentityModel.Tokens.Jwt;

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

    private readonly IAuthenticator _authenticator;
    private readonly B2COptions _options;

    public AuthService(IAuthenticator authenticator, B2COptions options)
    {
        _authenticator = authenticator;
        _options = options;
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