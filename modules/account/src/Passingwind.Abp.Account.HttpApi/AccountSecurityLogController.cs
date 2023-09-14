using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Passingwind.Abp.Identity;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("/api/account/security-logs")]
public class AccountSecurityLogController : AccountBaseController, IAccountSecurityLogAppService
{
    private readonly IAccountSecurityLogAppService _service;

    public AccountSecurityLogController(IAccountSecurityLogAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<PagedResultDto<IdentitySecurityLogDto>> GetListAsync(IdentitySecurityLogPagedListRequestDto input)
    {
        return _service.GetListAsync(input);
    }
}
