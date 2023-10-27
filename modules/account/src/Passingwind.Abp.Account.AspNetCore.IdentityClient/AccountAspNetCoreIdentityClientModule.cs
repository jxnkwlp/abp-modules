using Passingwind.Abp.IdentityClient;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.Account;

[DependsOn(
    typeof(AccountAspNetCoreModule),
    typeof(IdentityClientAspNetCoreModule)
)]
public class AccountAspNetCoreIdentityClientModule : AbpModule
{
}
