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
    ///  Get all tfs providers
    /// </summary>
    Task<ListResultDto<string>> GetAllProvidersAsync();

    /// <summary>
    ///  forget tfa client if available
    /// </summary>
    Task ForgetClientAsync();

    /// <summary>
    ///  Disabled account tfa state
    /// </summary>
    Task DisableAsync();

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
    ///  Verify authenticator token
    /// </summary>
    /// <param name="input"></param>
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
    /// <param name="input"></param>
    Task RemoveAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input);
}
