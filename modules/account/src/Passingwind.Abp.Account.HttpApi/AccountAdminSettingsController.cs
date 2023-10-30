using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("/api/account/admin/settings")]
public class AccountAdminSettingsController : AccountBaseController, IAccountAdminSettingsAppService
{
    private readonly IAccountAdminSettingsAppService _service;

    public AccountAdminSettingsController(IAccountAdminSettingsAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<AccountAdminSettingsDto> GetAsync()
    {
        return _service.GetAsync();
    }

    [HttpGet("captcha")]
    public Task<AccountCaptchaSettingsDto> GetCaptchaAsync()
    {
        return _service.GetCaptchaAsync();
    }

    [HttpGet("external-login")]
    public Task<AccountExternalLoginSettingsDto> GetExternalLoginAsync()
    {
        return _service.GetExternalLoginAsync();
    }

    [HttpGet("general")]
    public Task<AccountGeneralSettingsDto> GetGeneralAsync()
    {
        return _service.GetGeneralAsync();
    }

    [HttpGet("recaptcha")]
    public Task<AccountRecaptchaSettingsDto> GetRecaptchaAsync()
    {
        return _service.GetRecaptchaAsync();
    }

    [HttpPut]
    public Task UpdateAsync(AccountAdminSettingsDto input)
    {
        return _service.UpdateAsync(input);
    }

    [HttpPut("captcha")]
    public Task UpdateCaptchaAsync(AccountCaptchaSettingsDto input)
    {
        return _service.UpdateCaptchaAsync(input);
    }

    [HttpPut("external-login")]
    public Task UpdateExternalLoginAsync(AccountExternalLoginSettingsDto input)
    {
        return _service.UpdateExternalLoginAsync(input);
    }

    [HttpPut("general")]
    public Task UpdateGeneralAsync(AccountGeneralSettingsDto input)
    {
        return _service.UpdateGeneralAsync(input);
    }

    [HttpPut("recaptcha")]
    public Task UpdateRecaptchaAsync(AccountRecaptchaSettingsDto input)
    {
        return _service.UpdateRecaptchaAsync(input);
    }
}
