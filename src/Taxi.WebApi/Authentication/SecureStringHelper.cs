using System.Net;
using System.Security;

namespace Taxi.WebApi.Authentication;

public static class SecureStringHelper
{
    public static string FromSecureString(this SecureString value)
    {
        return new NetworkCredential(string.Empty, value).Password;
    }

    public static SecureString ToSecureString(this string value)
    {
        if (value is null)
        {
            return null!;
        }

        return new NetworkCredential(string.Empty, value).SecurePassword;
    }
}
