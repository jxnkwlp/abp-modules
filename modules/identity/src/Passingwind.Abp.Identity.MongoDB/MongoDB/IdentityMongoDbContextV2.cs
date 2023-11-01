using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.Identity.MongoDB;

[ConnectionStringName(AbpIdentityDbProperties.ConnectionStringName)]
public class IdentityMongoDbContextV2 : AbpMongoDbContext, IIdentityMongoDbContextV2
{
    public IMongoCollection<IdentityUserTwoFactor> UserTwoFactors => Collection<IdentityUserTwoFactor>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureIdentityV2();
    }
}
