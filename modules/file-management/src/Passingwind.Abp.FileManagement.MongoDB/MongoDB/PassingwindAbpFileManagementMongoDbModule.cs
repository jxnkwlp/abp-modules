﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.FileManagement.MongoDB;

[DependsOn(
    typeof(PassingwindAbpFileManagementDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class PassingwindAbpFileManagementMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<FileManagementMongoDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, MongoQuestionRepository>();
             */
        });
    }
}
