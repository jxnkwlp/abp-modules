using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileRepository : IRepository<File, Guid>
{
    Task<long> GetCountAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default);

    Task<List<File>> GetListAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<List<File>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<bool> IsFileNameExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<File?> FindByNameAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<File> GetByNameAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
}
