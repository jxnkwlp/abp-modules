using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.FileManagement;

public interface IFileHashCalculator : ISingletonDependency
{
    Task<string> GetAsync(byte[] bytes, CancellationToken cancellationToken = default);
}
