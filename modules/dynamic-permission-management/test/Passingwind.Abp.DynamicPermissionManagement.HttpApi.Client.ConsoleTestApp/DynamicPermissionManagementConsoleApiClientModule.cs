using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.DynamicPermissionManagement;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(DynamicPermissionManagementHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class DynamicPermissionManagementConsoleApiClientModule : AbpModule
{
}
