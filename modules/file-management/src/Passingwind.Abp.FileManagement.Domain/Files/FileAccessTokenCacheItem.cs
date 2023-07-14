using System;
using Volo.Abp.Caching;

namespace Passingwind.Abp.FileManagement.Files;

[CacheName("filemanagement:file:accesstoken")]
public class FileAccessTokenCacheItem
{
    public Guid FileId { get; }

    public FileAccessTokenCacheItem(Guid fileId)
    {
        FileId = fileId;
    }
}