using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.FileManagement.Files;

public class DefaultFileUniqueIdGenerator : IFileUniqueIdGenerator, ISingletonDependency
{
    public Task<string> CreateAsync(FileContainer fileContainer, Guid fileId, CancellationToken cancellationToken = default)
    {
        // return Nanoid.Nanoid.GenerateAsync(size: 32);
        return Task.FromResult(fileId.ToString("N"));
    }
}