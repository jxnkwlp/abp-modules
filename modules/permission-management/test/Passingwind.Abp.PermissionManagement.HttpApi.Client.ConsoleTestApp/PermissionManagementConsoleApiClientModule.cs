using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.PermissionManagement;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(PermissionManagementHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class PermissionManagementConsoleApiClientModule : AbpModule
{

}
