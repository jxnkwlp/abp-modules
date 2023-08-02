using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Passingwind.Abp.FileManagement.Files;

public class FileAccessToken : CreationAuditedAggregateRoot<Guid>
{
    protected FileAccessToken()
    {
    }

    public FileAccessToken(Guid id, Guid fileId, string token, DateTime? expirationTime = null) : base(id)
    {
        FileId = fileId;
        Token = token;
        ExpirationTime = expirationTime;
    }

    public Guid FileId { get; protected set; }

    public string Token { get; protected set; } = null!;

    public DateTime? ExpirationTime { get; protected set; }

    public uint DownloadCount { get; protected set; }

    public void SetDownloadCount(uint value)
    {
        DownloadCount = value;
    }
}
