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

public class DictionaryGroupRepository : EfCoreRepository<DictionaryManagementDbContext, DictionaryGroup, Guid>, IDictionaryGroupRepository
{
    public DictionaryGroupRepository(IDbContextProvider<DictionaryManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<DictionaryGroup?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<DictionaryGroup> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        var entity = await dbset.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        if (entity == null)
            throw new EntityNotFoundException(typeof(DictionaryGroup), name);

        return entity;
    }

    public virtual async Task<long> GetCountAsync(string? filter = null, string? parentName = null, bool? isPublic = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(!string.IsNullOrEmpty(parentName), x => x.ParentName == parentName)
            .WhereIf(isPublic.HasValue, x => x.IsPublic == isPublic)
            .LongCountAsync(cancellationToken);
    }

    public async Task<List<DictionaryGroup>> GetListAsync(string? filter = null, string? parentName = null, bool? isPublic = null, bool includeDetails = false, string? sorting = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(!string.IsNullOrEmpty(parentName), x => x.ParentName == parentName)
            .WhereIf(isPublic.HasValue, x => x.IsPublic == isPublic)
            .OrderBy(sorting ?? nameof(DictionaryItem.Name))
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<DictionaryGroup>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, string? parentName = null, bool? isPublic = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(!string.IsNullOrEmpty(parentName), x => x.ParentName == parentName)
            .WhereIf(isPublic.HasValue, x => x.IsPublic == isPublic)
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
