using System;
using Volo.Abp.Caching;

namespace Passingwind.Abp.FileManagement;

[CacheName("filemanagement:file:accesstoken")]
public class FileAccessTokenCacheItem
{
    public Guid TokenId { get; protected set; }
    public Guid FileId { get; protected set; }

    public DateTime ExpirationTime { get; protected set; }

    public FileAccessTokenCacheItem(Guid tokenId, Guid fileId, DateTime expirationTime)
    {
        TokenId = tokenId;
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
