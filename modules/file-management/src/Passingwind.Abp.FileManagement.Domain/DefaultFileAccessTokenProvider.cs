using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Timing;

namespace Passingwind.Abp.FileManagement;

public class DefaultFileAccessTokenProvider : IFileAccessTokenProvider, ITransientDependency
{
    protected IClock Clock { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IFileRepository FileRepository { get; }
    protected IDistributedCache<FileAccessTokenCacheItem> DistributedCache { get; }
    protected IFileAccessTokenRepository FileAccessTokenRepository { get; }

    public DefaultFileAccessTokenProvider(
        IClock clock,
        IFileRepository fileRepository,
        IDistributedCache<FileAccessTokenCacheItem> distributedCache,
        IFileAccessTokenRepository fileAccessTokenRepository,
        IGuidGenerator guidGenerator)
    {
        Clock = clock;
        FileRepository = fileRepository;
        DistributedCache = distributedCache;
        FileAccessTokenRepository = fileAccessTokenRepository;
        GuidGenerator = guidGenerator;
    }

    protected virtual Task<string> GenerateTokenAsync(FileContainer container, FileItem file, CancellationToken cancellationToken = default)
    {
        var token = Guid.NewGuid().ToString("N");
        return Task.FromResult(token);
    }

    public virtual async Task<FileAccessValidationResult> VerifyAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException($"'{nameof(token)}' cannot be null or whitespace.", nameof(token));
        }

        var fileCacheItem = await DistributedCache.GetOrAddAsync(token, async () =>
        {
            var entity = await FileAccessTokenRepository.FindAsync(x => x.Token == token, cancellationToken: cancellationToken);
            return entity != null
                ? new FileAccessTokenCacheItem(entity.Id, entity.FileId, entity.ExpirationTime ?? DateTime.MaxValue)
                : FileAccessTokenCacheItem.Null();
        }, () => new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = Clock.Now.AddDays(1),
        }, hideErrors: true, token: cancellationToken);

        if (fileCacheItem != null && fileCacheItem.ExpirationTime > Clock.Now)
        {
            var file = await FileRepository.FindAsync(fileCacheItem.FileId, cancellationToken: cancellationToken);

            if (file != null)
            {
                return FileAccessValidationResult.Valid(fileCacheItem.TokenId, file, fileCacheItem.ExpirationTime);
            }
        }

        return new FileAccessValidationResult();
    }
}
