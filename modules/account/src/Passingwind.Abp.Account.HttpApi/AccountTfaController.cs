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
    public Task<AccountTfaDto> GetAsync()
    {
        return TfaAppService.GetAsync();
    }

    [HttpGet("providers")]
    public Task<ListResultDto<string>> GetProvidersAsync()
    {
        return TfaAppService.GetProvidersAsync();
    }

    [HttpDelete("forget-client")]
    public Task ForgetClientAsync()
    {
        return TfaAppService.ForgetClientAsync();
    }

    [HttpDelete]
    public Task DisableAsync()
    {
        return TfaAppService.DisableAsync();
    }

    [HttpPost("{provider}/verify")]
    public Task<AccountVerifyTokenResultDto> VerifyTokenAsync(string provider, AccountTfaVerifyTokenRequestDto input)
    {
        return TfaAppService.VerifyTokenAsync(provider, input);
    }

    [HttpGet("authenticator")]
    public Task<AccountAuthenticatorInfoDto> GetAuthenticatorAsync()
    {
        return TfaAppService.GetAuthenticatorAsync();
    }

    [HttpPut("authenticator")]
    public Task<AccountAuthenticatorRecoveryCodesResultDto> UpdateAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        return TfaAppService.UpdateAuthenticatorAsync(input);
    }

    [HttpPut("authenticator/recovery-codes")]
    public Task<AccountAuthenticatorRecoveryCodesResultDto> GenerateAuthenticatorRecoveryCodesAsync()
    {
        return TfaAppService.GenerateAuthenticatorRecoveryCodesAsync();
    }

    [HttpPut("authenticator/reset")]
    public Task ResetAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        return TfaAppService.ResetAuthenticatorAsync(input);
    }

    [HttpPost("{provider}/token")]
    public Task SendTokenAsync(string provider)
    {
        return TfaAppService.SendTokenAsync(provider);
    }
}
