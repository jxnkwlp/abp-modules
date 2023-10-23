using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Passingwind.Abp.IdentityClient.EntityFrameworkCore;

public static class IdentityClientDbContextModelCreatingExtensions
{
    public static void ConfigureIdentityClient(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder
            .Entity<IdentityClient>(b =>
            {
                b.ToTable(IdentityClientDbProperties.DbTablePrefix + "IdentityClients", IdentityClientDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(64);
                b.Property(x => x.ProviderName).IsRequired().HasMaxLength(128);
                b.Property(x => x.DisplayName).HasMaxLength(64);

                b.Property(x => x.RequiredClaimTypes).HasConversion(x => ToJson(x), x => ToObject(x, new List<string>())!, new ValueComparer<List<string>>(false));

                b.HasMany(x => x.Configurations).WithOne().HasForeignKey(x => x.IdentityClientId);
                b.HasMany(x => x.ClaimMaps).WithOne().HasForeignKey(x => x.IdentityClientId);

                b.HasIndex(x => x.Name);
                b.HasIndex(x => x.ProviderName);
                b.HasIndex(x => x.CreationTime).IsDescending();
            })
            .Entity<IdentityClientConfiguration>(b =>
            {
                b.ToTable(IdentityClientDbProperties.DbTablePrefix + "IdentityClientConfigurations", IdentityClientDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(64);

                b.HasKey(x => new { x.IdentityClientId, x.Name });
            })
            .Entity<IdentityClientClaimMap>(b =>
            {
                b.ToTable(IdentityClientDbProperties.DbTablePrefix + "IdentityClientClaimMaps", IdentityClientDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.ClaimType).IsRequired().HasMaxLength(128);
                b.Property(x => x.ValueFromType).HasMaxLength(128);
                b.Property(x => x.RawValue).HasMaxLength(256);

                b.HasKey(x => new { x.IdentityClientId, x.ClaimType });
            });
    }

    static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

    static string ToJson(object value)
    {
        if (value == null)
            return string.Empty;

        return JsonSerializer.Serialize(value, JsonSerializerOptions);
    }

    static T? ToObject<T>(string value, T? defaultValue = default)
    {
        return JsonSerializer.Deserialize<T>(value, JsonSerializerOptions) ?? defaultValue;
    }
}
