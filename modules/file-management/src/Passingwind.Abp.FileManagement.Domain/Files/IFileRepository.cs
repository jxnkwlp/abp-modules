using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileRepository : IRepository<File, Guid>
{
    Task<long> GetCountAsync(string? filter, Guid? containerId, Guid? parentId, CancellationToken cancellationToken = default);

    Task<List<File>> GetListAsync(string? filter, Guid? containerId, Guid? parentId, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<List<File>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter, Guid? containerId, Guid? parentId, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<bool> IsFileNameExistsAsync(Guid containerId, string fileName, Guid? parentId, CancellationToken cancellationToken = default);
}
