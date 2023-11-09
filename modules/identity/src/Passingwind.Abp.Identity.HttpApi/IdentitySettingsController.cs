using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

[Area(IdentityRemoteServiceConsts.ModuleName)]
[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Route("api/identity/settings")]
public class IdentitySettingsController : IdentityBaseController, IIdentitySettingsAppService
{
    private readonly IIdentitySettingsAppService _service;

    public IdentitySettingsController(IIdentitySettingsAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<IdentitySettingsDto> GetAsync()
    {
        return _service.GetAsync();
    }

    [HttpGet("lockout")]
    public virtual Task<IdentityLockoutSettingsDto> GetLockoutAsync()
    {
        return _service.GetLockoutAsync();
    }

    [HttpGet("organization-unit")]
    public virtual Task<OrganizationUnitSettingsDto> GetOrganizationUnitAsync()
    {
        return _service.GetOrganizationUnitAsync();
    }

    [HttpGet("password")]
    public virtual Task<IdentityPasswordSettingsDto> GetPasswordAsync()
    {
        return _service.GetPasswordAsync();
    }

    [HttpGet("signin")]
    public virtual Task<IdentitySignInSettingsDto> GetSignInAsync()
    {
        return _service.GetSignInAsync();
    }

    [HttpGet("two-factor")]
    public virtual Task<IdentityTwofactorSettingsDto> GetTwofactorAsync()
    {
        return _service.GetTwofactorAsync();
    }

    [HttpGet("user")]
    public virtual Task<IdentityUserSettingsDto> GetUserAsync()
    {
        return _service.GetUserAsync();
    }

    [HttpPut]
    public virtual Task UpdateAsync(IdentitySettingsDto input)
    {
        return _service.UpdateAsync(input);
    }

    [HttpPut("lockout")]
    public virtual Task UpdateLockoutAsync(IdentityLockoutSettingsDto input)
    {
        return _service.UpdateLockoutAsync(input);
    }

    [HttpPut("organization-unit")]
    public virtual Task UpdateOrganizationUnitAsync(OrganizationUnitSettingsDto input)
    {
        return _service.UpdateOrganizationUnitAsync(input);
    }

    [HttpPut("password")]
    public virtual Task UpdatePasswordAsync(IdentityPasswordSettingsDto input)
    {
        return _service.UpdatePasswordAsync(input);
    }

    [HttpPut("signin")]
    public virtual Task UpdateSignInAsync(IdentitySignInSettingsDto input)
    {
        return _service.UpdateSignInAsync(input);
    }

    [HttpPut("two-factor")]
    public virtual Task UpdateTwofactorAsync(IdentityTwofactorSettingsDto input)
    {
        return _service.UpdateTwofactorAsync(input);
    }

    [HttpPut("user")]
    public virtual Task UpdateUserAsync(IdentityUserSettingsDto input)
    {
        return _service.UpdateUserAsync(input);
    }
}
