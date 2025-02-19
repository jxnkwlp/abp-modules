﻿using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public interface IAccountTfaAppService : IApplicationService
{
    /// <summary>
    ///  Get account tfa state
    /// </summary>
    Task<AccountTfaDto> GetAsync();

    /// <summary>
    ///  Get account tfa providers
    /// </summary>
    Task<ListResultDto<string>> GetProvidersAsync();

    /// <summary>
    ///  forget tfa client if available
    /// </summary>
    Task ForgetClientAsync();

    /// <summary>
    ///  Disabled account tfa state
    /// </summary>
    Task DisableAsync();

    /// <summary>
    ///  enable tfs
    /// </summary>
    Task EnabledAsync();

    /// <summary>
    ///  Get authenticator state
    /// </summary>
    Task<AccountAuthenticatorInfoDto> GetAuthenticatorAsync();
    /// <summary>
    ///  Update authenticator state
    /// </summary>
    Task<AccountAuthenticatorRecoveryCodesResultDto> UpdateAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input);
    /// <summary>
    ///  Verify authenticator token
    /// </summary>
    Task<AccountVerifyTokenResultDto> VerifyAuthenticatorTokenAsync(AccountAuthenticatorCodeVerifyRequestDto input);
    /// <summary>
    ///  Regenerate authenticator recovery codes
    /// </summary>
    /// <remarks>
    ///  Generating new recovery codes does not change the keys used in authenticator apps
    /// </remarks>
    Task<AccountAuthenticatorRecoveryCodesResultDto> GenerateAuthenticatorRecoveryCodesAsync();
    /// <summary>
    ///  Remove authenticator provider
    /// </summary>
    /// <remarks>
    ///  remove your authenticator key your authenticator app will not work until you reconfigure it
    /// </remarks>
    Task RemoveAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input);

    /// <summary>
    ///  enable email token provider
    /// </summary>
    Task EnabledEmailTokenProviderAsync();
    /// <summary>
    ///  Disable the provider
    /// </summary>
    Task DisabledEmailTokenProviderAsync();
    /// <summary>
    ///  enable phone number token provider
    /// </summary>
    Task EnabledPhoneNumberTokenProviderAsync();
    /// <summary>
    ///  Disable the provider
    /// </summary>
    Task DisabledPhoneNumberTokenProviderAsync();

    Task<AccountPreferredProviderDto> GetPreferredProviderAsync();
    Task UpdatePreferredProviderAsync(AccountUpdatePreferredProviderDto input);
}
