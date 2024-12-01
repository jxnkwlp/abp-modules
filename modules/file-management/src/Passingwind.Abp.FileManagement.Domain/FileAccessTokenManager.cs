using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace Passingwind.Abp.FileManagement;

public class FileAccessTokenManager : DomainService
{
    private readonly IFileAccessTokenRepository _fileAccessTokenRepository;

    public FileAccessTokenManager(IFileAccessTokenRepository fileAccessTokenRepository)
    {
        _fileAccessTokenRepository = fileAccessTokenRepository;
    }

    [UnitOfWork]
    public virtual async Task DeleteAllExpirationTokenAsync(CancellationToken cancellationToken = default)
    {
        var endTime = Clock.Now;

        await _fileAccessTokenRepository.DeleteDirectAsync(x => x.ExpirationTime != null && x.ExpirationTime < endTime, cancellationToken);
    }
}
