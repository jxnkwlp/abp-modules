using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("/api/account/admin/settings")]
public class AccountAdminSettingController : AccountBaseController, IAccountAdminSettingAppService
{
    private readonly IAccountAdminSettingAppService _service;

    public AccountAdminSettingController(IAccountAdminSettingAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<AccountAdminSettingsDto> GetAsync()
    {
        return _service.GetAsync();
    }

    [HttpPut]
    public Task UpdateAsync(AccountAdminSettingsDto input)
    {
        return _service.UpdateAsync(input);
    }
}
