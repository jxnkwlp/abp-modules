using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace Passingwind.Abp.FileManagement;

public interface IFileBlobContainerProvider
{
    Task<IBlobContainer> GetAsync(FileContainer container, CancellationToken cancellationToken = default);
}
