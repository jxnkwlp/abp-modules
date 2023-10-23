using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Passingwind.Abp.IdentityClient.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Passingwind.Abp.IdentityClient.Identity;

public class ExternalUserProvider : IExternalUserProvider, ITransientDependency, IUnitOfWorkEnabled
{
    private static readonly Regex RegexEmail = new(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

    protected IGuidGenerator GuidGenerator { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected IdentityUserManager UserManager { get; }
    protected IdentityClientOption IdentityClientOptions { get; set; }

    public ExternalUserProvider(
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant,
        IOptions<IdentityOptions> identityOptions,
        IdentityUserManager userManager,
        IOptions<IdentityClientOption> identityClientOptions)
    {
        GuidGenerator = guidGenerator;
        CurrentTenant = currentTenant;
        IdentityOptions = identityOptions;
        UserManager = userManager;
        IdentityClientOptions = identityClientOptions.Value;
    }

    public virtual async Task<IdentityUser> CreateUserAsync(ClaimsPrincipal principal, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
    {
        await IdentityOptions.SetAsync();

        var userName = GetUserNameFromClaims(principal);
        var emailAddress = GetEmailAddressFromClaims(principal);

        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new Exception("Can't resolve username from claims.");
        }

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
        CheckIdentityErrors(await UserManager.AddLoginAsync(user, new UserLoginInfo(loginProvider, providerKey, null)));

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

        if (user != null) return user;

        if (IdentityOptions.Value.User.RequireUniqueEmail && !string.IsNullOrWhiteSpace(emailAddress))
            user = await UserManager.FindByEmailAsync(emailAddress);

        return user;
    }

    public virtual async Task<IdentityUser> UpdateUserAsync(IdentityUser identityUser, ClaimsPrincipal principal, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
    {
        await IdentityOptions.SetAsync();

        // logins 
        if (await UserManager.FindByLoginAsync(loginProvider, providerKey) == null)
        {
            CheckIdentityErrors(await UserManager.AddLoginAsync(identityUser, new UserLoginInfo(loginProvider, providerKey, null)));
        }

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

    protected virtual string GetUserNameFromClaims(ClaimsPrincipal principal)
    {
        var userName = principal.FindFirstValue("preferred_username");

        if (string.IsNullOrEmpty(userName))
            userName = principal.FindFirstValue(ClaimTypes.Name);

        if (string.IsNullOrEmpty(userName))
            userName = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userName))
        {
            if (!IdentityClientOptions.GenerateUserName)
                throw new Exception("Missing username in claims.");

            userName = $"user{RandomHelper.GetRandom(10000, 99999)}";
        }

        return userName;
    }

    protected virtual string? GetEmailAddressFromClaims(ClaimsPrincipal principal, bool generate = false)
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
