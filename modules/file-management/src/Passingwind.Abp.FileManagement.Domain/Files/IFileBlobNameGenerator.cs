using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileBlobNameGenerator
{
    Task<string> CreateAsync(Guid containerId, Guid fileId, string uniqueId, string fileName, string mimeType, long length, string hash, CancellationToken cancellationToken = default);
}
