using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("/api/account/dynamic-claims")]
public class AccountDynamicClaimsController : AbpControllerBase, IDynamicClaimsAppService
{
    protected IDynamicClaimsAppService DynamicClaimsAppService { get; }

    public AccountDynamicClaimsController(IDynamicClaimsAppService dynamicClaimsAppService)
    {
        DynamicClaimsAppService = dynamicClaimsAppService;
    }

    [HttpPost]
    [Route("refresh")]
    public virtual Task RefreshAsync()
    {
        return DynamicClaimsAppService.RefreshAsync();
    }
}
