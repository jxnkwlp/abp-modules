using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("/api/account/admin/settings")]
public class AccountAdminSettingsController : AbpControllerBase, IAccountAdminSettingsAppService
{
    private readonly IAccountAdminSettingsAppService _service;

    public AccountAdminSettingsController(IAccountAdminSettingsAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<AccountAdminSettingsDto> GetAsync()
    {
        return _service.GetAsync();
    }

    [HttpGet("captcha")]
    public virtual Task<AccountCaptchaSettingsDto> GetCaptchaAsync()
    {
        return _service.GetCaptchaAsync();
    }

    [HttpGet("external-login")]
    public virtual Task<AccountExternalLoginSettingsDto> GetExternalLoginAsync()
    {
        return _service.GetExternalLoginAsync();
    }

    [HttpGet("general")]
    public virtual Task<AccountGeneralSettingsDto> GetGeneralAsync()
    {
        return _service.GetGeneralAsync();
    }

    [HttpGet("recaptcha")]
    public virtual Task<AccountRecaptchaSettingsDto> GetRecaptchaAsync()
    {
        return _service.GetRecaptchaAsync();
    }

    [HttpPut]
    public virtual Task UpdateAsync(AccountAdminSettingsDto input)
    {
        return _service.UpdateAsync(input);
    }

    [HttpPut("captcha")]
    public virtual Task UpdateCaptchaAsync(AccountCaptchaSettingsDto input)
    {
        return _service.UpdateCaptchaAsync(input);
    }

    [HttpPut("external-login")]
    public virtual Task UpdateExternalLoginAsync(AccountExternalLoginSettingsDto input)
    {
        return _service.UpdateExternalLoginAsync(input);
    }

    [HttpPut("general")]
    public virtual Task UpdateGeneralAsync(AccountGeneralSettingsDto input)
    {
        return _service.UpdateGeneralAsync(input);
    }

    [HttpPut("recaptcha")]
    public virtual Task UpdateRecaptchaAsync(AccountRecaptchaSettingsDto input)
    {
        return _service.UpdateRecaptchaAsync(input);
    }
}
