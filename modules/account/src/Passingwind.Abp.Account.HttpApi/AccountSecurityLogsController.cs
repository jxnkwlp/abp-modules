using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Passingwind.Abp.Identity;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("/api/account/security-logs")]
public class AccountSecurityLogsController : AbpControllerBase, IAccountSecurityLogsAppService
{
    private readonly IAccountSecurityLogsAppService _service;

    public AccountSecurityLogsController(IAccountSecurityLogsAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<IdentitySecurityLogsDto>> GetListAsync([FromQuery] AccountSecurityLogsPagedListRequestDto input)
    {
        return _service.GetListAsync(input);
    }
}
