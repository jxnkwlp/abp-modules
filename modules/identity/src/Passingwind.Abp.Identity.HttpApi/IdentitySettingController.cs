using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

[Area(IdentityRemoteServiceConsts.ModuleName)]
[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Route("api/identity/settings")]
public class IdentitySettingController : IdentityBaseController, IIdentitySettingAppService
{
    private readonly IIdentitySettingAppService _service;

    public IdentitySettingController(IIdentitySettingAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<IdentitySettingsDto> GetAsync()
    {
        return _service.GetAsync();
    }

    [HttpPut]
    public virtual Task UpdateAsync(IdentitySettingsDto input)
    {
        return _service.UpdateAsync(input);
    }
}
