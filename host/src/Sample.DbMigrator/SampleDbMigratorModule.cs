using Sample.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Sample.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SampleEntityFrameworkCoreModule),
    typeof(SampleApplicationContractsModule)
    )]
public class SampleDbMigratorModule : AbpModule
{
}
