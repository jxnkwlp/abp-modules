using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Passingwind.Abp.FileManagement.Files;

public class DefaultFileAccessTokenProvider : IFileAccessTokenProvider, ITransientDependency
{
    private readonly IFileRepository _fileRepository;
    private readonly IDistributedCache<FileAccessTokenCacheItem> _distributedCache;
    private readonly IClock _clock;

    public DefaultFileAccessTokenProvider(IDistributedCache<FileAccessTokenCacheItem> distributedCache, IClock clock, IFileRepository fileRepository)
    {
        _distributedCache = distributedCache;
        _clock = clock;
        _fileRepository = fileRepository;
    }

    public async Task<string> CreateAsync(FileContainer container, File file, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        string token = Guid.NewGuid().ToString("N");

        await _distributedCache.SetAsync(token, new FileAccessTokenCacheItem(file.Id), new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = expiration.HasValue ? _clock.Now.Add(expiration.Value) : null,
        });

        return token;
    }

    public async Task<File?> ValidAsync(string token, CancellationToken cancellationToken = default)
    {
        var fileCacheItem = await _distributedCache.GetAsync(token, true);

        if (fileCacheItem != null)
        {
            return await _fileRepository.GetAsync(fileCacheItem.FileId);
        }

        return null;
    }
}
