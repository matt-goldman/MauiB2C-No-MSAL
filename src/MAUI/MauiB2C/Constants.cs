namespace MauiB2C;

public static partial class Constants
{
    public static string BaseUrl
    {
        get
        {
#if DEBUG
            return GetTunnelUrl();
#else
            return "[your production API URL]";
#endif
        }
    }

    private static partial string GetTunnelUrl();
}
