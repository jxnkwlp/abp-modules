using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.FileManagement;

public interface IFileRepository : IRepository<FileItem, Guid>
{
    Task<long> GetCountAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default);

    Task<List<FileItem>> GetListAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<List<FileItem>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<List<FileItem>> GetListByIdsAsync(IEnumerable<Guid> ids, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<bool> IsFileNameExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<FileItem?> FindByNameAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<FileItem> GetByNameAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
}
