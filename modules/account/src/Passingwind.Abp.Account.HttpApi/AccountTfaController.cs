using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("/api/account/2fa")]
public class AccountTfaController : AccountBaseController, IAccountTfaAppService
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

    [HttpGet("providers/all")]
    public virtual Task<ListResultDto<string>> GetAllProvidersAsync()
    {
        return TfaAppService.GetAllProvidersAsync();
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
    public virtual Task RemoveAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        return TfaAppService.RemoveAuthenticatorAsync(input);
    }

    [HttpPost("authenticator/verify")]
    public virtual Task<AccountVerifyTokenResultDto> VerifyAuthenticatorTokenAsync(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        return TfaAppService.VerifyAuthenticatorTokenAsync(input);
    }
}
