using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement;

public interface IFileBlobNameGenerator
{
    Task<string> CreateAsync(Guid containerId, Guid fileId, string uniqueId, string fileName, string? mimeType = null, long? length = null, string? hash = null, CancellationToken cancellationToken = default);
}
