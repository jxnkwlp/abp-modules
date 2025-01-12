using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement;

public interface IFileAccessTokenProvider
{
    Task<FileAccessValidationResult> VerifyAsync(string token, CancellationToken cancellationToken = default);
}
