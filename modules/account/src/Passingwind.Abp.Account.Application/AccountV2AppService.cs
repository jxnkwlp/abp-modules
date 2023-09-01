using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Account;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IAccountAppService))]
public class AccountV2AppService : AccountAppService
{
    public AccountV2AppService(
        IdentityUserManager userManager,
        IIdentityRoleRepository roleRepository,
        IAccountEmailer accountEmailer,
        IdentitySecurityLogManager identitySecurityLogManager,
        IOptions<IdentityOptions> identityOptions) : base(userManager, roleRepository, accountEmailer, identitySecurityLogManager, identityOptions)
    {
    }

    public override async Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input)
    {
        await IdentityOptions.SetAsync();

        // if email is not unique, can't use this function
        // TODO

        if (!IdentityOptions.Value.User.RequireUniqueEmail)
        {
            throw new UserFriendlyException("this feature disabled");
        }

        await base.SendPasswordResetCodeAsync(input);
    }
}
