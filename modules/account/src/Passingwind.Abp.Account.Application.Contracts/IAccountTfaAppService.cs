using System.Threading.Tasks;
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
    ///  Send token for specify token provider
    /// </summary>
    /// <param name="provider"></param>
    Task SendTokenAsync(string provider);

    /// <summary>
    ///  Verify token for specify token provider
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="input"></param>
    Task<AccountVerifyTokenResultDto> VerifyTokenAsync(string provider, AccountTfaVerifyTokenRequestDto input);

    /// <summary>
    ///  Get authenticator state
    /// </summary>
    Task<AccountAuthenticatorInfoDto> GetAuthenticatorAsync();
    /// <summary>
    ///  Update authenticator state
    /// </summary>
    /// <param name="input"></param>
    Task<AccountAuthenticatorRecoveryCodesResultDto> UpdateAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input);
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
    /// <param name="input"></param>
    Task ResetAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input);
}
