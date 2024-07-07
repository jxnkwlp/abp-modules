using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.DictionaryManagement.Dictionaries;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.DictionaryManagement.EntityFrameworkCore.Repositories;

public class DictionaryItemRepository : EfCoreRepository<DictionaryManagementDbContext, DictionaryItem, Guid>, IDictionaryItemRepository
{
    public DictionaryItemRepository(IDbContextProvider<DictionaryManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<DictionaryItem?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<DictionaryItem> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        var entity = await dbset.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        if (entity == null)
            throw new EntityNotFoundException(typeof(DictionaryItem), name);

        return entity;
    }

    public virtual async Task<long> GetCountAsync(string? filter = null, string? groupName = null, bool? isEnabled = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(!string.IsNullOrEmpty(groupName), x => x.GroupName == groupName)
            .WhereIf(isEnabled.HasValue, x => x.IsEnabled == isEnabled)
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<DictionaryItem>> GetListAsync(string? filter = null, string? groupName = null, bool? isEnabled = null, bool includeDetails = false, string? sorting = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(!string.IsNullOrEmpty(groupName), x => x.GroupName == groupName)
            .WhereIf(isEnabled.HasValue, x => x.IsEnabled == isEnabled)
            .OrderBy(sorting ?? nameof(DictionaryItem.Name))
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<DictionaryItem>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, string? groupName = null, bool? isEnabled = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(!string.IsNullOrEmpty(groupName), x => x.GroupName == groupName)
            .WhereIf(isEnabled.HasValue, x => x.IsEnabled == isEnabled)
            .OrderBy(sorting ?? nameof(DictionaryItem.Name))
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsNameExistsAsync(string name, IEnumerable<Guid>? excludeIds = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(excludeIds?.Any() == true, x => !excludeIds!.Contains(x.Id))
            .AnyAsync(x => x.Name == name, cancellationToken);
    }
}
