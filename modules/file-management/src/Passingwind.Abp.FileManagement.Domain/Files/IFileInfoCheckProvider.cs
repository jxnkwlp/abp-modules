using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileInfoCheckProvider : ITransientDependency
{
    /// <summary>
    ///  check file or directory when create or update
    /// </summary>
    Task CheckAsync(FileContainer container, File file, CancellationToken cancellationToken = default);
}
