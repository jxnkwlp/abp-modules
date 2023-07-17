using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileBlobContainerProvider
{
    Task<IBlobContainer> GetAsync(FileContainer container, CancellationToken cancellationToken = default);
}
