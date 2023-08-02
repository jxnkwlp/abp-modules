using System;
using Volo.Abp.Caching;

namespace Passingwind.Abp.FileManagement.Files;

[CacheName("filemanagement:file:accesstoken")]
public class FileAccessTokenCacheItem
{
    public Guid FileId { get; protected set; }

    public DateTime? ExpirationTime { get; protected set; }

    public FileAccessTokenCacheItem(Guid fileId, DateTime? expirationTime)
    {
        FileId = fileId;
        ExpirationTime = expirationTime;
    }

    protected FileAccessTokenCacheItem()
    {
    }

    public static FileAccessTokenCacheItem Null()
    {
        return new FileAccessTokenCacheItem() { ExpirationTime = DateTime.MinValue };
    }
}
