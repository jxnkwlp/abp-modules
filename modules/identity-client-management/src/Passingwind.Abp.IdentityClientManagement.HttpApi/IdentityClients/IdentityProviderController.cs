using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

[Area(IdentityClientManagementRemoteServiceConsts.ModuleName)]
[RemoteService(Name = IdentityClientManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/identity-providers")]
public class IdentityProviderController : IdentityClientManagementController, IIdentityProviderAppService
{
    private readonly IIdentityProviderAppService _service;

    public IdentityProviderController(IIdentityProviderAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<ListResultDto<IdentityProviderDto>> GetListAsync()
    {
        return _service.GetListAsync();
    }
}
