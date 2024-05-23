using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Passingwind.Abp.IdentityClient;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.Account;

[ApiExplorerSettings(IgnoreApi = true)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("auth/external")]
public class AccountExternalAuthController : AbpController
{
    protected IIdentityClientLoginAppService IdentityClientLoginAppService { get; }

    public AccountExternalAuthController(IIdentityClientLoginAppService identityClientLoginAppService)
    {
        IdentityClientLoginAppService = identityClientLoginAppService;
    }

    [AllowAnonymous]
    [HttpGet("identity/{provider}/login")]
    public virtual async Task LoginAsync(string provider, string? returnUrl = null, string? returnUrlHash = null)
    {
        var redirectUrl = Url.Action("callback", values: new { returnUrl, returnUrlHash });

        await IdentityClientLoginAppService.LoginAsync(provider, redirectUrl);
    }
}
