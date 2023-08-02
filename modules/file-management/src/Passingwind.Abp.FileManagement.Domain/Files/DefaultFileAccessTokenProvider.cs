using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Timing;

namespace Passingwind.Abp.FileManagement.Files;

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

#pragma warning disable RCS1163 // Unused parameter.
    protected virtual Task<string> GenerateTokenAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
#pragma warning restore RCS1163 // Unused parameter.
    {
        string token = Guid.NewGuid().ToString("N");
        return Task.FromResult(token);
    }

    public virtual async Task<string> CreateAsync(FileContainer container, File file, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        string token = await GenerateTokenAsync(container, file, cancellationToken);

        DateTime? expirationTime = expiration.HasValue ? Clock.Now.Add(expiration.Value) : null;

        var checheItem = new FileAccessTokenCacheItem(file.Id, expirationTime);
        var cacheOptions = new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = expirationTime,
        };

        // add to cache
        await DistributedCache.SetAsync(token, checheItem, cacheOptions, token: cancellationToken);

        // add record to DB
        await FileAccessTokenRepository.InsertAsync(new FileAccessToken(GuidGenerator.Create(), file.Id, token, expirationTime), cancellationToken: cancellationToken);

        return token;
    }

    public virtual async Task<FileAccessValidationResult> ValidAsync(string token, CancellationToken cancellationToken = default)
    {
        // priority from cache
        var fileCacheItem = await DistributedCache.GetAsync(token);

        // if cache lost
        if (fileCacheItem == null)
        {
            var entity = await FileAccessTokenRepository.FindAsync(x => x.Token == token);
            if (entity != null)
            {
                fileCacheItem = new FileAccessTokenCacheItem(entity.FileId, entity.ExpirationTime);
                var cacheOptions = new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = entity.ExpirationTime,
                };

                await DistributedCache.SetAsync(token, fileCacheItem, cacheOptions, token: cancellationToken);
            }
            else
            {
                var cacheOptions = new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = Clock.Now.AddDays(1),
                };

                // set 'null' cache
                await DistributedCache.SetAsync(token, FileAccessTokenCacheItem.Null(), options: cacheOptions, token: cancellationToken);
            }
        }

        if (fileCacheItem != null && fileCacheItem.ExpirationTime > Clock.Now)
        {
            var file = await FileRepository.FindAsync(fileCacheItem.FileId);

            if (file != null)
            {
                return FileAccessValidationResult.Valid(file, fileCacheItem.ExpirationTime);
            }
        }

        return new FileAccessValidationResult();
    }
}
