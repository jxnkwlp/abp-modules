using System;

namespace Passingwind.Abp.FileManagement;

public class FileAccessValidationResult
{
    public Guid TokenId { get; protected set; }
    public bool IsValid { get; protected set; }
    public DateTime? ExpirationTime { get; protected set; }
    public FileItem? File { get; protected set; }

    public FileAccessValidationResult()
    {
        IsValid = false;
    }

    public static FileAccessValidationResult Valid(Guid tokenId, FileItem file, DateTime? ExpirationTime)
    {
        return new FileAccessValidationResult
        {
            TokenId = tokenId,
            IsValid = true,
            File = file,
            ExpirationTime = ExpirationTime,
        };
    }
}
