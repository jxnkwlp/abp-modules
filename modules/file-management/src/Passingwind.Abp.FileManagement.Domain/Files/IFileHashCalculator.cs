using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileHashCalculator : ISingletonDependency
{
    Task<string> GetAsync(byte[] bytes, CancellationToken cancellationToken = default);
}
