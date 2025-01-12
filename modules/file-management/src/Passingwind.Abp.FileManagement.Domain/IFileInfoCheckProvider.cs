using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.FileManagement;

public interface IFileInfoCheckProvider : ITransientDependency
{
    /// <summary>
    ///  check file or directory when create or update
    /// </summary>
    Task CheckAsync(FileContainer container, string fileName, string mimeType, long length, CancellationToken cancellationToken = default);
    Task<bool> IsValidAsync(FileContainer container, string fileName, string mimeType, long length, CancellationToken cancellationToken = default);

    Task<bool> IsFileNameValidAsync(string fileName, CancellationToken cancellationToken = default);
}
