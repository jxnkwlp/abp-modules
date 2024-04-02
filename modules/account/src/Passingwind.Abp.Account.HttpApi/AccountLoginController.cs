using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.Account;

[Area(AccountRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Route("api/account")]
public class AccountLoginController : AbpControllerBase, IAccountLoginAppService
{
    private readonly IAccountLoginAppService _service;

    public AccountLoginController(IAccountLoginAppService service)
    {
        _service = service;
    }

    [HttpPost("change-password")]
    public virtual Task ChangePasswordAsync(AccountRequiredChangePasswordRequestDto input)
    {
        return _service.ChangePasswordAsync(input);
    }

    [HttpPost("check-password")]
    public virtual Task<AccountLoginResultDto> CheckPasswordAsync(AccountLoginRequestDto input)
    {
        return _service.CheckPasswordAsync(input);
    }

    [HttpGet("login/2fa")]
    public virtual Task<AccountTFaStateDto> GetTfaStatusAsync()
    {
        return _service.GetTfaStatusAsync();
    }

    [HttpGet("authenticator")]
    public virtual Task<AccountAuthenticatorInfoDto> GetAuthenticatorInfoAsync()
    {
        return _service.GetAuthenticatorInfoAsync();
    }

    [HttpGet("login/external-providers")]
    public virtual Task<ListResultDto<AccountExternalAuthenticationSchameDto>> GetExternalAuthenticationsAsync()
    {
        return _service.GetExternalAuthenticationsAsync();
    }

    [HttpGet("authenticator/status")]
    public virtual Task<AccountHasAuthenticatorResultDto> HasAuthenticatorAsync()
    {
        return _service.HasAuthenticatorAsync();
    }

    [HttpPost("login")]
    public virtual Task<AccountLoginResultDto> LoginAsync(AccountLoginRequestDto input)
    {
        return _service.LoginAsync(input);
    }

    [HttpPost("login/2fa/{provider}")]
    public virtual Task<AccountLoginResultDto> LoginWithTfaAsync(string provider, AccountLoginWithTfaRequestDto input)
    {
        return _service.LoginWithTfaAsync(provider, input);
    }

    [HttpPost("login/authenticator/recovery-code")]
    public virtual Task<AccountLoginResultDto> LoginWithAuthenticatorRecoveryCodeAsync(AccountLoginWithAuthenticatorRecoveryCodeRequestDto input)
    {
        return _service.LoginWithAuthenticatorRecoveryCodeAsync(input);
    }

    [Obsolete]
    [HttpGet("logout")]
    public virtual Task GetLogoutAsync()
    {
        return _service.LogoutAsync();
    }

    [HttpPost("logout")]
    public virtual Task LogoutAsync()
    {
        return _service.LogoutAsync();
    }

    [HttpPost("login/2fa/{provider}/token")]
    public virtual Task SendTfaTokenAsync(string provider)
    {
        return _service.SendTfaTokenAsync(provider);
    }

    [HttpPost("authenticator/verify")]
    public virtual Task<AccountAuthenticatorRecoveryCodesResultDto> VerifyAuthenticatorToken(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        return _service.VerifyAuthenticatorToken(input);
    }

    [HttpPost("login/2fa/{provider}/verify")]
    public virtual Task<AccountVerifyTokenResultDto> VerifyTfaTokenAsync(string provider, AccountLoginVerifyTwoFactorTokenDto input)
    {
        return _service.VerifyTfaTokenAsync(provider, input);
    }
}
