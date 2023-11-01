using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Passingwind.Abp.Identity.TokenProviders;

public class PasswordlessLoginTokenProvider<TUser> : TotpSecurityStampBasedTokenProvider<TUser> where TUser : class
{
    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
    {
        return Task.FromResult(false);
    }

    public override async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        var userId = await manager.GetUserIdAsync(user);

        return "PasswordlessLogin:" + purpose + ":" + userId;
    }
}
