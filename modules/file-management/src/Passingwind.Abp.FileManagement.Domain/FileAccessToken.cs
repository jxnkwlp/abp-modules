using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.FileManagement;

public class FileAccessToken : CreationAuditedAggregateRoot<Guid>, IMultiTenant
{
    protected FileAccessToken()
    {
    }

    public FileAccessToken(Guid id, Guid containerId, Guid fileId, string fileName, long length, string? mimeType, string token, DateTime? expirationTime, Guid? tenantId = null) : base(id)
    {
        ContainerId = containerId;
        FileId = fileId;
        FileName = fileName;
        Length = length;
        MimeType = mimeType;
        Token = token;
        ExpirationTime = expirationTime;
        TenantId = tenantId;
    }

    public Guid ContainerId { get; protected set; }
    public Guid FileId { get; protected set; }
    public string FileName { get; protected set; } = null!;
    public long Length { get; protected set; }
    public string? MimeType { get; protected set; }

    public string Token { get; protected set; } = null!;
    public DateTime? ExpirationTime { get; protected set; }

    public uint DownloadCount { get; protected set; }

    public Guid? TenantId { get; protected set; }

    public void SetDownloadCount(uint value)
    {
        DownloadCount = value;
    }
}
