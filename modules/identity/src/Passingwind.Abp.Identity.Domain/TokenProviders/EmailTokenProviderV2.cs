using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Identity.Options;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity.TokenProviders;

/// <summary>
/// TokenProvider that generates tokens from the user's security stamp and notifies a user via email.
/// </summary>
/// <typeparam name="TUser"></typeparam>
public class EmailTokenProviderV2<TUser> : EmailTokenProvider<TUser> where TUser : class
{
    protected IdentityUserTwoFactorManager UserTwoFactorManager { get; }
    protected IOptions<IdentityUserTokenOptions> UserTokenOptions { get; }

    public EmailTokenProviderV2(IdentityUserTwoFactorManager userTwoFactorManager, IOptions<IdentityUserTokenOptions> userTokenOptions)
    {
        UserTwoFactorManager = userTwoFactorManager;
        UserTokenOptions = userTokenOptions;
    }

    /// <summary>
    /// Checks if a two-factor authentication token can be generated for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="user"></param>
    /// <exception cref="ArgumentNullException"><paramref name="manager"/> is <c>null</c>.</exception>
    public override async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
    {
        if (manager == null)
            throw new ArgumentNullException(nameof(manager));

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var requireConfirmedEmail = UserTokenOptions.Value.RequireConfirmedEmail;

        var confirmed = await manager.IsEmailConfirmedAsync(user);

        var enabled = await UserTwoFactorManager.GetEmailTokenEnabledAsync((user as IdentityUser)!);

        var email = await manager.GetEmailAsync(user).ConfigureAwait(false);

        var result = !string.IsNullOrWhiteSpace(email) && enabled;

        if (requireConfirmedEmail)
        {
            return result && confirmed;
        }

        return result;
    }
}
