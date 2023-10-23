using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.IdentityClient;

[Area(IdentityClientRemoteServiceConsts.ModuleName)]
[RemoteService(Name = IdentityClientRemoteServiceConsts.RemoteServiceName)]
[Route("api/identity-providers")]
public class IdentityProviderController : IdentityClientControllerBase, IIdentityProviderAppService
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
