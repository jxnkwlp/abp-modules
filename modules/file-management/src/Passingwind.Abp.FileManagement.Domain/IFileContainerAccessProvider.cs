using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement;

public interface IFileContainerAccessProvider
{
    Task<bool> IsGrantedAsync(string name, string providerName, Guid providerId, CancellationToken cancellationToken = default);
    Task<bool> IsGrantedAsync(Guid containerId, string providerName, Guid providerId, CancellationToken cancellationToken = default);
}
