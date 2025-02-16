using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.AspNetCore.Mvc;

[DependsOn(typeof(AbpAspNetCoreMvcModule))]
[DependsOn(typeof(AspNetCoreMvcModuleContracts))]
public class AspNetCoreMvcModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}
