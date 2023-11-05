using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("api/account/claims")]
public class AccountClaimsController : AccountBaseController, IAccountClaimsAppService
{
    protected IAccountClaimsAppService Service { get; }

    public AccountClaimsController(IAccountClaimsAppService service)
    {
        Service = service;
    }

    /// <inheritdoc/>
    [HttpGet]
    public virtual Task<ListResultDto<AccountClaimResultDto>> GetListAsync()
    {
        return Service.GetListAsync();
    }
}
