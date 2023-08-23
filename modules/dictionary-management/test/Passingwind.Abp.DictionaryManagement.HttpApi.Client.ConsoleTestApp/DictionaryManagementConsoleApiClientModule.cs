using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.DictionaryManagement;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(DictionaryManagementHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class DictionaryManagementConsoleApiClientModule : AbpModule
{
}
