using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Passingwind.Abp.ApiKey.EntityFrameworkCore;

public static class ApiKeyDbContextModelCreatingExtensions
{
    public static void ConfigureApiKey(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<ApiKeyRecord>(b =>
        {
            b.ToTable(ApiKeyDbProperties.DbTablePrefix + "ApiKeys", ApiKeyDbProperties.DbSchema);
            b.ConfigureByConvention();

            //Properties
            b.Property(x => x.Name).IsRequired().HasMaxLength(ApiKeyConsts.MaxApiKeyNameLength);
            b.Property(x => x.Secret).IsRequired().HasMaxLength(ApiKeyConsts.MaxApiKeySecretLength);

            //Indexes
            b.HasIndex(q => q.CreationTime).IsDescending();
            b.HasIndex(q => q.Name);
            b.HasIndex(q => q.Secret);
        });
    }
}
