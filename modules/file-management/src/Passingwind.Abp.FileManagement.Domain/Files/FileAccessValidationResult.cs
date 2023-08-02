using System;

namespace Passingwind.Abp.FileManagement.Files;

public class FileAccessValidationResult
{
    public bool IsValid { get; protected set; }
    public DateTime? ExpirationTime { get; protected set; }
    public File? File { get; protected set; }

    public FileAccessValidationResult()
    {
        IsValid = false;
    }

    public static FileAccessValidationResult Valid(File file, DateTime? ExpirationTime)
    {
        return new FileAccessValidationResult
        {
            IsValid = true,
            File = file,
            ExpirationTime = ExpirationTime,
        };
    }
}
