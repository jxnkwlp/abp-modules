using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Passingwind.Abp.Account;

public class AccountV2AppService : AccountAppService, IAccountV2AppService
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

    public virtual async Task<AccountVerifyPasswordResetTokenResultDto> VerifyPasswordResetTokenV2Async(VerifyPasswordResetTokenInput input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(input.UserId);
        var result = await UserManager.VerifyUserTokenAsync(
            user,
            UserManager.Options.Tokens.PasswordResetTokenProvider,
            UserManager<IdentityUser>.ResetPasswordTokenPurpose,
            input.ResetToken);

        return new AccountVerifyPasswordResetTokenResultDto()
        {
            Verified = result,
        };
    }
}
