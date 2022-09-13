# Azure AD  B2C in .NET MAUI *without* MSAL
Sample demonstrating how you can use AAD B2C in your .NET MAUI apps without using MSAL.

Watch the video:    
<a href="http://www.youtube.com/watch?feature=player_embedded&v=gQoqg4P-uJ0" target="_blank">
 <img src="http://img.youtube.com/vi/gQoqg4P-uJ0/hqdefault.jpg" alt="Watch the video" />
</a>
## Context

MSAL is not currently available for .NET MAUI apps (see this issue: https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/issues/3127).

But you can use AAD B2C (or any OAuth2 or OIDC compliant IDP) in your .NET MAUI apps using (mostly) built in tools.

**NOTE:** Feel free to use this in your .NET MAUI apps. But bear in mind that this is [technical debt](https://www.youtube.com/watch?v=ASVD4YIOgpU&t=0s), so you should keep an eye on the above issue, and when its resolved remove this custom implementation and switch to MSAL.

## Other links and resources:
AADB2C: https://docs.microsoft.com/en-us/azure/active-directory-b2c/overview
WinUIEx: https://github.com/dotMorten/WinUIEx/
Postman: https://www.postman.com/
DevToys: https://devtoys.app/

MSAL GitHub issue: https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/issues/3127
WebAuthenticator issue: https://github.com/microsoft/WindowsAppSDK/issues/441
SecureStorage issue: https://github.com/dotnet/maui/issues/8326
