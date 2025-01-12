using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.FileManagement;

public class DefaultFileUniqueIdGenerator : IFileUniqueIdGenerator, ISingletonDependency
{
    public virtual Task<string> CreateAsync(Guid containerId, Guid fileId, string fileName, bool isDirectory, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(fileId.ToString("N"));
    }
}