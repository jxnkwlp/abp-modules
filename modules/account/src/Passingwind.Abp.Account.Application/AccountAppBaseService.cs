using System;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.Account.Localization;
using Passingwind.Abp.Identity.Settings;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public abstract class AccountAppBaseService : ApplicationService
{
    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    protected UrlEncoder UrlEncoder => LazyServiceProvider.GetRequiredService<UrlEncoder>();

    protected AccountAppBaseService()
    {
        LocalizationResource = typeof(AccountResource);
        ObjectMapperContext = typeof(AccountApplicationModule);
    }

    protected static string FormatKey(string unformattedKey)
    {
        var result = new StringBuilder();
        int currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
            currentPosition += 4;
        }
        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition));
        }

        return result.ToString().ToLowerInvariant();
    }

    protected virtual async Task<string> GenerateAuthenticatorQrCodeUri(string identitifer, string unformattedKey)
    {
        var issuer = await SettingProvider.GetOrNullAsync(IdentitySettingNamesV2.Twofactor.AuthenticatorIssuer);

        if (string.IsNullOrWhiteSpace(issuer))
            issuer = "MYAPP";

        return string.Format(
            CultureInfo.InvariantCulture,
            AuthenticatorUriFormat,
            issuer,
            UrlEncoder.Encode(identitifer),
            unformattedKey);
    }
}
