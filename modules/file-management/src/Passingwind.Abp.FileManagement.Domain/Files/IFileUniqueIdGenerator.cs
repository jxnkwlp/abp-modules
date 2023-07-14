using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileUniqueIdGenerator
{
    Task<string> CreateAsync(FileContainer fileContainer, CancellationToken cancellationToken = default);
}
