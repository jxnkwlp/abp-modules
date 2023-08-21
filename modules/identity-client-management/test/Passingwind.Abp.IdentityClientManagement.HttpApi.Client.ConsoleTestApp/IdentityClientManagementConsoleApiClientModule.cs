using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClientManagement;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(IdentityClientManagementHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class IdentityClientManagementConsoleApiClientModule : AbpModule
{

}
