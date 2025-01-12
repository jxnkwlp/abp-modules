using System;

namespace Passingwind.Abp.FileManagement;

public class FileShareResultDto
{
    public Guid Id { get; set; }
    public Guid ContainerId { get; set; }
    public string? ContainerName { get; set; }
    public string FileName { get; set; } = null!;
    public long Length { get; set; }
    public string? MimeType { get; set; }
    public DateTime? ExpirationTime { get; set; }
    public string? Token { get; set; }
    public string? DirectDownloadUrl { get; set; }
}
