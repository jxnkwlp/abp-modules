using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Passingwind.Abp.Identity.TokenProviders;

/// <summary>
/// Used for authenticator code verification.
/// </summary>
/// <typeparam name="TUser"></typeparam>
public class AuthenticatorTokenProviderV2<TUser> : AuthenticatorTokenProvider<TUser> where TUser : class
{
    /// <summary>
    /// Checks if a two-factor authentication token can be generated for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="user"></param>
    public override async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
    {
        var key = await manager.GetAuthenticatorKeyAsync(user);
        var recoveryCodesCount = await manager.CountRecoveryCodesAsync(user);

        return !string.IsNullOrWhiteSpace(key) && recoveryCodesCount > 0;
    }
}
