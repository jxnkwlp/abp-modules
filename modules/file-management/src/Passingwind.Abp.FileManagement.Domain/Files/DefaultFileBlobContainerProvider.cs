using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Options;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.FileManagement.Files;

public class DefaultFileBlobContainerProvider : IFileBlobContainerProvider, ITransientDependency
{
    private readonly IBlobContainerFactory _blobContainerFactor;
    private readonly FileManagementOptions _options;

    public DefaultFileBlobContainerProvider(IBlobContainerFactory blobContainerFactor, IOptions<FileManagementOptions> options)
    {
        _blobContainerFactor = blobContainerFactor;
        _options = options.Value;
    }

    public virtual Task<IBlobContainer> GetAsync(FileContainer container, CancellationToken cancellationToken = default)
    {
        IBlobContainer blobContainer;

        if (_options.FileContainerAsBlobContainer)
            blobContainer = _blobContainerFactor.Create(container.Name);
        else
            blobContainer = _blobContainerFactor.Create(_options.DefaultBlobContainer);

        return Task.FromResult(blobContainer);
    }
}