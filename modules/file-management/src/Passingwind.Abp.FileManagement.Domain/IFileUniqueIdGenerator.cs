using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement;

public interface IFileUniqueIdGenerator
{
    Task<string> CreateAsync(Guid containerId, Guid fileId, string fileName, bool isDirectory, CancellationToken cancellationToken = default);
}
