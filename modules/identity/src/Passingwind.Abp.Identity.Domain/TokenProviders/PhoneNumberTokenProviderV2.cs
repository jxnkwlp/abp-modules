using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Identity.Options;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity.TokenProviders;

/// <summary>
/// Represents a token provider that generates tokens from a user's security stamp and
/// sends them to the user via their phone number.
/// </summary>
public class PhoneNumberTokenProviderV2 : PhoneNumberTokenProvider<IdentityUser>
{
    protected IdentityUserTwoFactorManager UserTwoFactorManager { get; }
    protected IOptions<IdentityUserTokenOptions> UserTokenOptions { get; }

    public PhoneNumberTokenProviderV2(IdentityUserTwoFactorManager userTwoFactorManager, IOptions<IdentityUserTokenOptions> userTokenOptions)
    {
        UserTwoFactorManager = userTwoFactorManager;
        UserTokenOptions = userTokenOptions;
    }

    /// <summary>
    /// Returns a flag indicating whether the token provider can generate a token suitable for two-factor authentication token for
    /// the specified <paramref name="user"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="manager"/> is <c>null</c>.</exception>
    public override async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<IdentityUser> manager, IdentityUser user)
    {
        if (manager == null)
        {
            throw new ArgumentNullException(nameof(manager));
        }

        var requireConfirmedPhoneNumber = UserTokenOptions.Value.RequireConfirmedPhoneNumber;

        var confirmed = await manager.IsPhoneNumberConfirmedAsync(user);

        var enabled = await UserTwoFactorManager.GetPhoneNumberTokenEnabledAsync(user!);

        var phoneNumber = await manager.GetPhoneNumberAsync(user);

        var result = !string.IsNullOrWhiteSpace(phoneNumber) && enabled;

        if (requireConfirmedPhoneNumber)
        {
            return result && confirmed;
        }

        return result;
    }
}
