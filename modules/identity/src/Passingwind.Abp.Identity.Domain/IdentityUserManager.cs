using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Passingwind.Abp.Identity;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(Volo.Abp.Identity.IdentityUserManager))]
public class IdentityUserManager : Volo.Abp.Identity.IdentityUserManager
{
    public IdentityUserManager(
        IdentityUserStore store,
        IIdentityRoleRepository roleRepository,
        IIdentityUserRepository userRepository,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<IdentityUser> passwordHasher,
        IEnumerable<IUserValidator<IdentityUser>> userValidators,
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<Volo.Abp.Identity.IdentityUserManager> logger,
        ICancellationTokenProvider cancellationTokenProvider,
        IOrganizationUnitRepository organizationUnitRepository,
        ISettingProvider settingProvider,
        IDistributedEventBus distributedEventBus,
        IIdentityLinkUserRepository identityLinkUserRepository,
        IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache) : base(store, roleRepository, userRepository, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger, cancellationTokenProvider, organizationUnitRepository, settingProvider, distributedEventBus, identityLinkUserRepository, dynamicClaimCache)
    {
    }

    /// <summary>
    ///  Check user should change password
    /// </summary>
    public virtual async Task<bool> ShouldChangePasswordAsync(IdentityUser user)
    {
        ThrowIfDisposed();
        Check.NotNull(user, nameof(user));

        if (user.PasswordHash.IsNullOrWhiteSpace())
        {
            return false;
        }

        if (user.ShouldChangePasswordOnNextLogin)
            return true;

        var forceUsersToPeriodicallyChangePassword = await SettingProvider.GetAsync<bool>(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword);

        if (!forceUsersToPeriodicallyChangePassword)
            return false;

        var lastPasswordChangeTime = user.LastPasswordChangeTime ?? DateTime.SpecifyKind(user.CreationTime, DateTimeKind.Utc);
        var passwordChangePeriodDays = await SettingProvider.GetAsync<int>(IdentitySettingNames.Password.PasswordChangePeriodDays);

        return passwordChangePeriodDays > 0 && lastPasswordChangeTime.AddDays(passwordChangePeriodDays) < DateTime.UtcNow;
    }

    /// <summary>
    ///  Remove an authentication token for a user. both 'AuthenticatorKey' and 'RecoveryCodes'
    /// </summary>
    public virtual async Task RemoveAuthenticatorAsync(IdentityUser user)
    {
        ThrowIfDisposed();
        Check.NotNull(user, nameof(user));

        await RemoveAuthenticationTokenAsync(user, "[AspNetUserStore]", "AuthenticatorKey");
        await RemoveAuthenticationTokenAsync(user, "[AspNetUserStore]", "RecoveryCodes");
    }

    public virtual async Task<bool> VerifyEmailConfirmationTokenAsync(IdentityUser user, string token)
    {
        ThrowIfDisposed();
        Check.NotNull(user, nameof(user));

        return await VerifyUserTokenAsync(user, Options.Tokens.EmailConfirmationTokenProvider, ConfirmEmailTokenPurpose, token).ConfigureAwait(false);
    }
}
