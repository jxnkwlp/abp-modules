using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Account;

[Area(AccountRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Route("api/account")]
public class AccountLoginController : AccountBaseController, IAccountLoginAppService
{
    private readonly IAccountLoginAppService _service;

    public AccountLoginController(IAccountLoginAppService service)
    {
        _service = service;
    }

    [HttpPost("change-password")]
    public Task ChangePasswordAsync(AccountRequiredChangePasswordRequestDto input)
    {
        return _service.ChangePasswordAsync(input);
    }

    [HttpPost("check-password")]
    public Task<AccountLoginResultDto> CheckPasswordAsync(AccountLoginRequestDto input)
    {
        return _service.CheckPasswordAsync(input);
    }

    [HttpGet("login/authenticator")]
    public Task<AccountAuthenticatorInfoDto> GetAuthenticatorInfoAsync()
    {
        return _service.GetAuthenticatorInfoAsync();
    }

    [HttpGet("login/external-providers")]
    public Task<ListResultDto<AccountExternalAuthenticationSchameDto>> GetExternalAuthenticationsAsync()
    {
        return _service.GetExternalAuthenticationsAsync();
    }

    [HttpGet("login/has-authenticator")]
    public Task<AccountHasAuthenticatorResultDto> HasAuthenticatorAsync()
    {
        return _service.HasAuthenticatorAsync();
    }

    [HttpPost("login")]
    public Task<AccountLoginResultDto> LoginAsync(AccountLoginRequestDto input)
    {
        return _service.LoginAsync(input);
    }

    [HttpPost("login/2fa")]
    public Task<AccountLoginResultDto> LoginWith2faAsync(AccountLoginWith2FaRequestDto input)
    {
        return _service.LoginWith2faAsync(input);
    }

    [HttpPost("login/recovery-code")]
    public Task<AccountLoginResultDto> LoginWithRecoveryCodeAsync(AccountLoginWithRecoveryCodeRequestDto input)
    {
        return _service.LoginWithRecoveryCodeAsync(input);
    }

    [HttpPost("logout")]
    public Task LogoutAsync()
    {
        return _service.LogoutAsync();
    }

    [HttpPost("authenticator/verify")]
    public Task<AccountAuthenticatorRecoveryCodesResultDto> VerifyAuthenticatorToken(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        return _service.VerifyAuthenticatorToken(input);
    }
}
