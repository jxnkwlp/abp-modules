using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Passingwind.Abp.Identity;

public class ExternalUserProvider : IExternalUserProvider, IScopedDependency, IUnitOfWorkEnabled
{
    private static readonly Regex RegexEmail = new(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

    protected IGuidGenerator GuidGenerator { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected IdentityUserManager UserManager { get; }

    public ExternalUserProvider(
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant,
        IOptions<IdentityOptions> identityOptions,
        IdentityUserManager userManager)
    {
        GuidGenerator = guidGenerator;
        CurrentTenant = currentTenant;
        IdentityOptions = identityOptions;
        UserManager = userManager;
    }

    public virtual async Task<IdentityUser> CreateUserAsync(ClaimsPrincipal principal, string loginProvider, string providerKey, string? loginDisplayName = null, bool? generateUserName = false, CancellationToken cancellationToken = default)
    {
        await IdentityOptions.SetAsync();

        var userName = GetUserNameFromClaims(principal, generateUserName.GetValueOrDefault());
        var emailAddress = GetEmailAddressFromClaims(principal);

        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            emailAddress = GenerateEmailAddress(principal, userName, loginProvider, providerKey);
        }

        var user = new IdentityUser(GuidGenerator.Create(), userName, emailAddress, CurrentTenant.Id);

        // create
        CheckIdentityErrors(await UserManager.CreateAsync(user));

        // default role
        CheckIdentityErrors(await UserManager.AddDefaultRolesAsync(user));

        // logins
        CheckIdentityErrors(await UserManager.AddLoginAsync(user, new UserLoginInfo(loginProvider, providerKey, loginDisplayName)));

        // others
        user.Name = principal.FindFirstValue(ClaimTypes.GivenName);
        user.Surname = principal.FindFirstValue(ClaimTypes.Surname);
        user.IsExternal = true;

        var phoneNumber = principal.FindFirstValue(AbpClaimTypes.PhoneNumber);
        if (!phoneNumber.IsNullOrWhiteSpace())
        {
            var phoneNumberConfirmed = string.Equals(principal.FindFirstValue(AbpClaimTypes.PhoneNumberVerified), "true", StringComparison.InvariantCultureIgnoreCase);
            user.SetPhoneNumber(phoneNumber, phoneNumberConfirmed);
        }

        await UserManager.UpdateAsync(user);

        return user;
    }

    public virtual async Task<IdentityUser?> FindUserAsync(ClaimsPrincipal principal, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
    {
        await IdentityOptions.SetAsync();

        // find from Logins
        var user = await UserManager.FindByLoginAsync(loginProvider, providerKey);

        if (user != null)
            return user;

        // try find from username & emailaddress
        var userName = GetUserNameFromClaims(principal);
        var emailAddress = GetEmailAddressFromClaims(principal);

        if (!string.IsNullOrWhiteSpace(userName))
            user = await UserManager.FindByNameAsync(userName);

        if (user != null)
            return user;

        if (IdentityOptions.Value.User.RequireUniqueEmail && !string.IsNullOrWhiteSpace(emailAddress))
            user = await UserManager.FindByEmailAsync(emailAddress);

        return user;
    }

    public virtual async Task<IdentityUser> UpdateUserAsync(IdentityUser identityUser, ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        await IdentityOptions.SetAsync();

        // others
        identityUser.Name = principal.FindFirstValue(ClaimTypes.GivenName);
        identityUser.Surname = principal.FindFirstValue(ClaimTypes.Surname);

        var phoneNumber = principal.FindFirstValue(AbpClaimTypes.PhoneNumber);
        if (!phoneNumber.IsNullOrWhiteSpace())
        {
            var phoneNumberConfirmed = string.Equals(principal.FindFirstValue(AbpClaimTypes.PhoneNumberVerified), "true", StringComparison.InvariantCultureIgnoreCase);
            identityUser.SetPhoneNumber(phoneNumber, phoneNumberConfirmed);
        }

        await UserManager.UpdateAsync(identityUser);

        return identityUser;
    }

    protected virtual void CheckIdentityErrors(IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            throw new UserFriendlyException("Operation failed: " + identityResult.Errors.Select(e => $"[{e.Code}] {e.Description}").JoinAsString(", "));
        }
    }

    protected virtual string GetUserNameFromClaims(ClaimsPrincipal principal, bool generate = false)
    {
        var userName = principal.FindFirstValue("preferred_username");

        if (string.IsNullOrEmpty(userName))
            userName = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userName))
            userName = principal.FindFirstValue(ClaimTypes.Name);

        if (string.IsNullOrWhiteSpace(userName))
            userName = GetEmailAddressFromClaims(principal);

        if (!string.IsNullOrWhiteSpace(userName))
        {
            return userName;
        }

        if (generate)
        {
            return $"user{RandomHelper.GetRandom(10000, 99999)}";
        }

        throw new BusinessException(IdentityErrorCodesV2.UserExternalLoginUserNameNotFound);
    }

    protected virtual string? GetEmailAddressFromClaims(ClaimsPrincipal principal)
    {
        var emailAddress = principal.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(emailAddress))
            emailAddress = principal.FindFirstValue("email");

        return emailAddress;
    }

    protected virtual string GenerateEmailAddress(ClaimsPrincipal principal, string userName, string loginProvider, string providerKey)
    {
        if (RegexEmail.IsMatch(userName!))
        {
            return userName;
        }

        return $"{userName}@{loginProvider.ToUpperInvariant()}.idp";
    }
}
