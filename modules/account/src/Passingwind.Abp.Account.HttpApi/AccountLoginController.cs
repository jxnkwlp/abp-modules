﻿using System.Threading.Tasks;
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

    [HttpGet("login/2fa")]
    public Task<AccountTFaStateDto> GetTfaStatusAsync()
    {
        return _service.GetTfaStatusAsync();
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

    [HttpPost("login/2fa/{provider}")]
    public Task<AccountLoginResultDto> LoginWithTfaAsync(string provider, AccountLoginWith2FaRequestDto input)
    {
        return _service.LoginWithTfaAsync(provider, input);
    }

    [HttpPost("login/authenticator/recovery-code")]
    public Task<AccountLoginResultDto> LoginWithAuthenticatorRecoveryCodeAsync(AccountLoginWithAuthenticatorRecoveryCodeRequestDto input)
    {
        return _service.LoginWithAuthenticatorRecoveryCodeAsync(input);
    }

    [HttpPost("logout")]
    public Task LogoutAsync()
    {
        return _service.LogoutAsync();
    }

    [HttpPost("login/2fa/{provider}/token")]
    public Task SendTfaTokenAsync(string provider)
    {
        return _service.SendTfaTokenAsync(provider);
    }

    [HttpPost("authenticator/verify")]
    public Task<AccountAuthenticatorRecoveryCodesResultDto> VerifyAuthenticatorToken(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        return _service.VerifyAuthenticatorToken(input);
    }

    [HttpPost("login/2fa/{provider}/verify")]
    public Task<AccountVerifyTokenResultDto> VerifyTfaTokenAsync(string provider, AccountLoginVerifyTwoFactorTokenDto input)
    {
        return _service.VerifyTfaTokenAsync(provider, input);
    }
}
