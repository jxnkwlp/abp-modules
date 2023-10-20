using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

/// <summary>
///  Account login application service
/// </summary>
public interface IAccountLoginAppService : IApplicationService
{
    #region Login

    /// <summary>
    ///  Logout
    /// </summary>
    Task LogoutAsync();

    /// <summary>
    ///  Password login
    /// </summary>
    /// <param name="input"></param>
    Task<AccountLoginResultDto> LoginAsync(AccountLoginRequestDto input);

    /// <summary>
    ///  Login with 2fa code
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="input"></param>
    Task<AccountLoginResultDto> LoginWithTfaAsync(string provider, AccountLoginWithTfaRequestDto input);

    /// <summary>
    ///  Login with authenticator recovery code
    /// </summary>
    /// <param name="input"></param>
    Task<AccountLoginResultDto> LoginWithAuthenticatorRecoveryCodeAsync(AccountLoginWithAuthenticatorRecoveryCodeRequestDto input);

    /// <summary>
    ///  Gets a collection of the known external login providers.
    /// </summary>
    Task<ListResultDto<AccountExternalAuthenticationSchameDto>> GetExternalAuthenticationsAsync();

    /// <summary>
    ///  Check password
    /// </summary>
    /// <param name="input"></param>
    Task<AccountLoginResultDto> CheckPasswordAsync(AccountLoginRequestDto input);

    #endregion Login

    #region tfa

    /// <summary>
    ///  Get user 2fa status when requires 2fa
    /// </summary>
    Task<AccountTFaStateDto> GetTfaStatusAsync();

    /// <summary>
    ///  Send 2fa token when requires 2fa
    /// </summary>
    /// <param name="provider"></param>
    Task SendTfaTokenAsync(string provider);

    /// <summary>
    ///  verify 2fa token
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="input"></param>
    Task<AccountVerifyTokenResultDto> VerifyTfaTokenAsync(string provider, AccountLoginVerifyTwoFactorTokenDto input);

    #endregion tfa

    #region ChangePassword

    /// <summary>
    ///  Change password
    /// </summary>
    /// <param name="input"></param>
    Task ChangePasswordAsync(AccountRequiredChangePasswordRequestDto input);

    #endregion ChangePassword

    #region Authenticator

    /// <summary>
    ///  Check tfa authentication user hash configure authenticator
    /// </summary>
    Task<AccountHasAuthenticatorResultDto> HasAuthenticatorAsync();

    /// <summary>
    ///  Get tfa authentication user authenticator info
    /// </summary>
    Task<AccountAuthenticatorInfoDto> GetAuthenticatorInfoAsync();

    /// <summary>
    ///  Tfa authentication user verify authenticator code
    /// </summary>
    /// <param name="input"></param>
    Task<AccountAuthenticatorRecoveryCodesResultDto> VerifyAuthenticatorToken(AccountAuthenticatorCodeVerifyRequestDto input);

    #endregion Authenticator

}
