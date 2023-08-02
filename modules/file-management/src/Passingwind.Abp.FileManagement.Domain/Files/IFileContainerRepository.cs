using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileContainerRepository : IRepository<FileContainer, Guid>
{
    Task<long> GetCountAsync(string? filter, Guid? userId = null, CancellationToken cancellationToken = default);

    Task<List<FileContainer>> GetListAsync(string? filter = null, Guid? userId = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<List<FileContainer>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? userId = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<bool> CheckExistsAsync(FileContainer fileContainer, CancellationToken cancellationToken = default);

    Task<FileContainer?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<FileContainer> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
