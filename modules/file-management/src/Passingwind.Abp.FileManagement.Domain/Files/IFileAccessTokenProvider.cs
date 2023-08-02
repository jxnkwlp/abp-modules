using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileAccessTokenProvider
{
    Task<string> CreateAsync(FileContainer container, File file, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

    Task<FileAccessValidationResult> ValidAsync(string token, CancellationToken cancellationToken = default);
}
