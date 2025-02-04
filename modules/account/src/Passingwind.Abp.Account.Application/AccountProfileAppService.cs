﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Account.Settings;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Users;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Passingwind.Abp.Account;

[Authorize]
public class AccountProfileAppService : ProfileAppService, IAccountProfileAppService
{
    protected IdentitySecurityLogManager SecurityLogManager { get; }
    protected IAccountTwoFactorTokenSender AccountTwoFactorTokenSender { get; }

    public AccountProfileAppService(
        IdentityUserManager userManager,
        IOptions<IdentityOptions> identityOptions,
        IdentitySecurityLogManager securityLogManager,
        IAccountTwoFactorTokenSender accountTwoFactorTokenSender) : base(userManager, identityOptions)
    {
        SecurityLogManager = securityLogManager;
        AccountTwoFactorTokenSender = accountTwoFactorTokenSender;
    }

    public virtual async Task<AccountProfileDto> GetV2Async()
    {
        var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());

        return ObjectMapper.Map<IdentityUser, AccountProfileDto>(currentUser);
    }

    public override async Task ChangePasswordAsync(ChangePasswordInput input)
    {
        if (!await SettingProvider.GetAsync<bool>(AccountSettingNames.General.EnableChangePasswordOnProfile))
        {
            throw new BusinessException(AccountErrorCodes.ChangePasswordDisabled);
        }

        await base.ChangePasswordAsync(input);

        // add log
        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.ChangePassword,
            UserName = CurrentUser.Name,
        });
    }

    public virtual async Task SendEmailConfirmAsync()
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.FindByIdAsync(CurrentUser.GetId().ToString());

        if (user == null)
            throw new AbpAuthorizationException();

        var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);

#if DEBUG
        Logger.LogDebug("User with id '{id}' has been generated new token '{token}' for email confirmation'.", user.Id, token);
#endif
        await AccountTwoFactorTokenSender.SendEmailConfirmationTokenAsync(user, token);
    }

    public virtual async Task UpdateEmailConfirmAsync(AccountVerifyTokenRequestDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.FindByIdAsync(CurrentUser.GetId().ToString());

        if (user == null)
            throw new AbpAuthorizationException();

        var result = await UserManager.ConfirmEmailAsync(user, input.Token);

        if (result.Succeeded)
        {
            await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = "ConfirmEmail",
                UserName = user.UserName,
            });
        }
        else
        {
            throw new BusinessException(AccountErrorCodes.TokenInvalid);
        }
    }

    public virtual async Task<AccountVerifyTokenResultDto> VerifyEmailConfirmTokenAsync(AccountVerifyTokenRequestDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.FindByIdAsync(CurrentUser.GetId().ToString());

        if (user == null)
            throw new AbpAuthorizationException();

        var valid = await UserManager.VerifyUserTokenAsync(
            user: user,
            tokenProvider: IdentityOptions.Value.Tokens.EmailConfirmationTokenProvider,
            purpose: IdentityUserManager.ConfirmEmailTokenPurpose,
            token: input.Token);

        return new AccountVerifyTokenResultDto
        {
            Valid = valid,
        };
    }

    public virtual async Task SendChangePhoneNumberTokenAsync(AccountProfileChangePhoneNumberTokenDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.FindByIdAsync(CurrentUser.GetId().ToString());

        if (user == null)
            throw new AbpAuthorizationException();

        var token = await UserManager.GenerateChangePhoneNumberTokenAsync(user, input.PhoneNumber);
#if DEBUG
        Logger.LogDebug("User with id '{id}' has been generated new token '{token}' for change phone number'.", user.Id, token);
#endif
        await AccountTwoFactorTokenSender.SendChangePhoneNumberTokenAsync(user, input.PhoneNumber, token);
    }

    public virtual async Task<AccountVerifyTokenResultDto> VerifyChangePhoneNumberTokenAsync(AccountProfileChangePhoneNumberDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.FindByIdAsync(CurrentUser.GetId().ToString());

        if (user == null)
            throw new AbpAuthorizationException();

        var valid = await UserManager.VerifyChangePhoneNumberTokenAsync(
            user: user,
            token: input.Token,
            phoneNumber: input.PhoneNumber);

        return new AccountVerifyTokenResultDto
        {
            Valid = valid,
        };
    }

    public virtual async Task ChangePhoneNumberAsync(AccountProfileChangePhoneNumberDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.FindByIdAsync(CurrentUser.GetId().ToString());

        if (user == null)
            throw new AbpAuthorizationException();

        var result = await UserManager.ChangePhoneNumberAsync(
            user: user,
            phoneNumber: input.PhoneNumber,
            token: input.Token);

        if (result.Succeeded)
        {
            await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.ChangePhoneNumber,
                UserName = user.UserName,
            });
        }
        else
        {
            throw new BusinessException(AccountErrorCodes.TokenInvalid);
        }
    }

    public virtual async Task SendChangeEmailTokenAsync(AccountProfileChangeEmailTokenDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.FindByIdAsync(CurrentUser.GetId().ToString());

        if (user == null)
            throw new AbpAuthorizationException();

        var token = await UserManager.GenerateChangeEmailTokenAsync(user, input.Email);

#if DEBUG
        Logger.LogDebug("User with id '{id}' has been generated new token '{token}' for change email'.", user.Id, token);
#endif
        await AccountTwoFactorTokenSender.SendChangeEmailTokenAsync(user, input.Email, token);
    }

    public virtual async Task<AccountVerifyTokenResultDto> VerifyChangeEmailTokenAsync(AccountProfileChangeEmailDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.FindByIdAsync(CurrentUser.GetId().ToString());

        if (user == null)
            throw new AbpAuthorizationException();

        var valid = await UserManager.VerifyUserTokenAsync(user, IdentityOptions.Value.Tokens.ChangeEmailTokenProvider, IdentityUserManager.GetChangeEmailTokenPurpose(input.Email), input.Token);

        return new AccountVerifyTokenResultDto
        {
            Valid = valid,
        };
    }

    public virtual async Task ChangeEmailAsync(AccountProfileChangeEmailDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.FindByIdAsync(CurrentUser.GetId().ToString());

        if (user == null)
            throw new AbpAuthorizationException();

        var result = await UserManager.ChangeEmailAsync(user, input.Email, input.Token);

        if (result.Succeeded)
        {
            await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.ChangeEmail,
                UserName = user.UserName,
            });
        }
        else
        {
            throw new BusinessException(AccountErrorCodes.TokenInvalid);
        }
    }
}
