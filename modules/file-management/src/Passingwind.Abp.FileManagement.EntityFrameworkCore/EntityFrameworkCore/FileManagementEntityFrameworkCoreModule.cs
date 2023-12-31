﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore;

[DependsOn(
    typeof(FileManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class FileManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<FileManagementDbContext>(options => options.AddDefaultRepositories());
    }
}
