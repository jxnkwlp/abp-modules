using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

/// <summary>
///  Account login application service
/// </summary>
public interface IAccountLoginAppService : IApplicationService
{
    /// <summary>
    ///  Password login
    /// </summary>
    /// <param name="input"></param>
    Task<AccountLoginResultDto> LoginAsync(AccountLoginRequestDto input);

    /// <summary>
    ///  Login with 2fa code
    /// </summary>
    /// <param name="input"></param>
    Task<AccountLoginResultDto> LoginWith2faAsync(AccountLoginWith2FaRequestDto input);

    /// <summary>
    ///  Login with 2fa recovery code
    /// </summary>
    /// <param name="input"></param>
    Task<AccountLoginResultDto> LoginWithRecoveryCodeAsync(AccountLoginWithRecoveryCodeRequestDto input);

    /// <summary>
    ///  Logout
    /// </summary>
    Task LogoutAsync();

    /// <summary>
    ///  Check password
    /// </summary>
    /// <param name="input"></param>
    Task<AccountLoginResultDto> CheckPasswordAsync(AccountLoginRequestDto input);

    /// <summary>
    ///  Change password
    /// </summary>
    /// <param name="input"></param>
    Task ChangePasswordAsync(AccountRequiredChangePasswordRequestDto input);
}
