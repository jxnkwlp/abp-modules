﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore.Repositories;

public class FileContainerRepository : EfCoreRepository<FileManagementDbContext, FileContainer, Guid>, IFileContainerRepository
{
    public FileContainerRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<bool> IsNameExistsAsync(string name, Guid[]? excludeIds = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(excludeIds?.Length > 0, x => !excludeIds!.Contains(x.Id))
            .AnyAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filter = null, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<FileContainer?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset.FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileContainer> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        var entity = await dbset.FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);

        return entity ?? throw new EntityNotFoundException();
    }

    public virtual async Task<List<FileContainer>> GetListByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<FileContainer>> GetListAsync(string? filter = null, Guid? userId = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<FileContainer>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? userId = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .OrderBy(sorting ?? nameof(FileContainer.CreationTime) + " desc")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task IncrementFileCountAsync(string name, int adjustment = 1, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        await dbset
            .Where(x => x.Name == name)
            .ExecuteUpdateAsync(x => x.SetProperty(s => s.FilesCount, s => s.FilesCount + adjustment), cancellationToken: cancellationToken);
    }
}
