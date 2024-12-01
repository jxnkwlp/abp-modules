using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.FileManagement;

public class FileContainerAccessProvider : IFileContainerAccessProvider, ITransientDependency
{
    public Task<bool> IsGrantedAsync(string name, string providerName, Guid providerId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    public Task<bool> IsGrantedAsync(Guid containerId, string providerName, Guid providerId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}
