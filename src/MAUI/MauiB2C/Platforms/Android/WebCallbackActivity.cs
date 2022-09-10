using Android.App;
using Android.Content;
using Android.Content.PM;

namespace MauiB2C.Platforms.Android;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter(new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "auth.mauib2c://auth",
    DataHost = "callback")]
public class WebCallbackActivity : WebAuthenticatorCallbackActivity
{

}
