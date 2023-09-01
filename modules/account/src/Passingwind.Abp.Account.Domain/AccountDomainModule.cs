using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.Account;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AccountDomainSharedModule)
)]
public class AccountDomainModule : AbpModule
{
}
