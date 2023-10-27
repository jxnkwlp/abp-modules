using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Passingwind.Abp.Identity;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("/api/account/security-logs")]
public class AccountSecurityLogsController : AccountBaseController, IAccountSecurityLogsAppService
{
    private readonly IAccountSecurityLogsAppService _service;

    public AccountSecurityLogsController(IAccountSecurityLogsAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<IdentitySecurityLogDto>> GetListAsync([FromQuery] AccountSecurityLogPagedListRequestDto input)
    {
        return _service.GetListAsync(input);
    }
}
