using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.AspNetCore.Mvc;

[DependsOn(typeof(AbpAspNetCoreMvcContractsModule))]
public class AspNetCoreMvcModuleContracts : AbpModule;
