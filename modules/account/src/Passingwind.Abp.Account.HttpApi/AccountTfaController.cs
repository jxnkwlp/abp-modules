using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("/api/account/2fa")]
public class AccountTfaController : AbpControllerBase, IAccountTfaAppService
{
    protected IAccountTfaAppService TfaAppService { get; }

    public AccountTfaController(IAccountTfaAppService tfaAppService)
    {
        TfaAppService = tfaAppService;
    }

    [HttpGet]
    public virtual Task<AccountTfaDto> GetAsync()
    {
        return TfaAppService.GetAsync();
    }

    [HttpGet("providers")]
    public virtual Task<ListResultDto<string>> GetProvidersAsync()
    {
        return TfaAppService.GetProvidersAsync();
    }

    [HttpDelete("forget-client")]
    public virtual Task ForgetClientAsync()
    {
        return TfaAppService.ForgetClientAsync();
    }

    [HttpDelete]
    public virtual Task DisableAsync()
    {
        return TfaAppService.DisableAsync();
    }

    [HttpGet("authenticator")]
    public virtual Task<AccountAuthenticatorInfoDto> GetAuthenticatorAsync()
    {
        return TfaAppService.GetAuthenticatorAsync();
    }

    [HttpPut("authenticator")]
    public virtual Task<AccountAuthenticatorRecoveryCodesResultDto> UpdateAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        return TfaAppService.UpdateAuthenticatorAsync(input);
    }

    [HttpPut("authenticator/recovery-codes")]
    public virtual Task<AccountAuthenticatorRecoveryCodesResultDto> GenerateAuthenticatorRecoveryCodesAsync()
    {
        return TfaAppService.GenerateAuthenticatorRecoveryCodesAsync();
    }

    [HttpDelete("authenticator")]
    public virtual Task RemoveAuthenticatorAsync([FromBody] AccountAuthenticatorCodeVerifyRequestDto input)
    {
        return TfaAppService.RemoveAuthenticatorAsync(input);
    }

    [HttpPost("authenticator/verify")]
    public virtual Task<AccountVerifyTokenResultDto> VerifyAuthenticatorTokenAsync(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        return TfaAppService.VerifyAuthenticatorTokenAsync(input);
    }

    [HttpPut]
    public virtual Task EnabledAsync()
    {
        return TfaAppService.EnabledAsync();
    }

    [HttpPut("providers/email")]
    public virtual Task EnabledEmailTokenProviderAsync()
    {
        return TfaAppService.EnabledEmailTokenProviderAsync();
    }

    [HttpPut("providers/phone-number")]
    public virtual Task EnabledPhoneNumberTokenProviderAsync()
    {
        return TfaAppService.EnabledPhoneNumberTokenProviderAsync();
    }

    [HttpDelete("providers/email")]
    public virtual Task DisabledEmailTokenProviderAsync()
    {
        return TfaAppService.DisabledEmailTokenProviderAsync();
    }

    [HttpDelete("providers/phone-number")]
    public virtual Task DisabledPhoneNumberTokenProviderAsync()
    {
        return TfaAppService.DisabledPhoneNumberTokenProviderAsync();
    }

    [HttpPut("preferred-provider")]
    public virtual Task UpdatePreferredProviderAsync(AccountUpdatePreferredProviderDto input)
    {
        return TfaAppService.UpdatePreferredProviderAsync(input);
    }

    [HttpGet("preferred-provider")]
    public virtual Task<AccountPreferredProviderDto> GetPreferredProviderAsync()
    {
        return TfaAppService.GetPreferredProviderAsync();
    }
}
