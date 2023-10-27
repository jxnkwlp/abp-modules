using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.IdentityClient;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.Account;

[ExposeServices(typeof(IAccountExternalProvider))]
public class IdentityClientExternalProvider : AccountExternalProvider
{
    protected IIdentityClientRepository IdentityClientRepository { get; }

    public IdentityClientExternalProvider(IOptions<AccountExternalLoginOptions> accountExternalOptions, ILogger<AccountExternalProvider> logger, IIdentityClientRepository identityClientRepository) : base(accountExternalOptions, logger)
    {
        IdentityClientRepository = identityClientRepository;
    }

    public override async Task LoginInfoReceivedAsync(AccountExternalCallbackLoginInfoContext context)
    {
        var providerName = context.LoginInfo.LoginProvider;

        // check is debug mode
        var identityClient = await IdentityClientRepository.FindByProviderNameAsync(providerName);

        // TODO check tenant 
        // 
        if (identityClient?.IsDebugMode == true)
        {
            Logger.LogWarning("YOU ARE USE DEBUG MODE FOR IDENTITY PROVIDER");
            context.Handled = true;
            context.Result = new ObjectResult(context.LoginInfo.Principal.Claims.Select(x => new { x.Type, x.Value }));
        }
    }
}
