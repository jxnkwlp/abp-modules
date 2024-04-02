using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("api/account/authentication/logins")]
public class AccountAuthenticationLoginController : AbpControllerBase, IAccountAuthenticationLoginAppService
{
    protected IAccountAuthenticationLoginAppService Service { get; }

    public AccountAuthenticationLoginController(IAccountAuthenticationLoginAppService service)
    {
        Service = service;
    }

    /// <inheritdoc/>
    [HttpGet]
    public virtual Task<ListResultDto<AccountAuthenticationLoginResultDto>> GetListAsync()
    {
        return Service.GetListAsync();
    }
}
