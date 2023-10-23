using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.IdentityClient.EntityFrameworkCore.Repositories;

public class IdentityClientRepository : EfCoreRepository<IdentityClientDbContext, IdentityClient, Guid>, IIdentityClientRepository
{
    public IdentityClientRepository(IDbContextProvider<IdentityClientDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<IdentityClient?> FindByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .IncludeIf(includeDetails, x => x.ClaimMaps)
            .IncludeIf(includeDetails, x => x.Configurations)
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    public virtual async Task<IdentityClient?> FindByProviderNameAsync(string providerName, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .IncludeIf(includeDetails, x => x.ClaimMaps)
            .IncludeIf(includeDetails, x => x.Configurations)
            .FirstOrDefaultAsync(x => x.ProviderName == providerName, cancellationToken: cancellationToken);
    }

    public virtual async Task<IdentityClient> GetByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        var entity = await dbset
            .IncludeIf(includeDetails, x => x.ClaimMaps)
            .IncludeIf(includeDetails, x => x.Configurations)
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);

        if (entity == null)
            throw new EntityNotFoundException(typeof(IdentityClient));

        return entity;
    }

    public virtual async Task<long> GetCountAsync(string? filter, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
             .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<IdentityClient>> GetListAsync(string? filter, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .IncludeIf(includeDetails, x => x.ClaimMaps)
            .IncludeIf(includeDetails, x => x.Configurations)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<IdentityClient>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .IncludeIf(includeDetails, x => x.ClaimMaps)
            .IncludeIf(includeDetails, x => x.Configurations)
            .OrderBy(sorting ?? nameof(IdentityClient.CreationTime) + " desc")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> IsNameExistsAsync(string name, Guid[]? excludeIds = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(excludeIds != null, x => !excludeIds!.Contains(x.Id))
            .AnyAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }
}
