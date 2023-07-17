using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileUniqueIdGenerator
{
    Task<string> CreateAsync(FileContainer fileContainer, Guid fileId, CancellationToken cancellationToken = default);
}
