using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.FileManagement;

public interface IFileAccessTokenRepository : IRepository<FileAccessToken, Guid>
{
    Task<long> GetCountAsync(Guid containerId, Guid? fileId = null, Guid? userId = null, CancellationToken cancellationToken = default);
    Task<List<FileAccessToken>> GetListAsync(Guid containerId, Guid? fileId = null, Guid? userId = null, CancellationToken cancellationToken = default);
    Task<List<FileAccessToken>> GetPagedListAsync(int skipCount, int maxResultCount, Guid containerId, Guid? fileId = null, Guid? userId = null, string? sorting = null, CancellationToken cancellationToken = default);
}
