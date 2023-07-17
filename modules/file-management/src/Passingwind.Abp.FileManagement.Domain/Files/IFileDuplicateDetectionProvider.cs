using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileDuplicateDetectionProvider
{
    Task<bool> IsExistsAsync(FileContainer fileContainer, File file, CancellationToken cancellationToken = default);
}
