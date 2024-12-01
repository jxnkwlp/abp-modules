using System;

namespace Passingwind.Abp.FileManagement;

public class FileShareCreateRequestDto
{
    /// <summary>
    ///  Expiration time for second
    /// </summary>
    public DateTime? ExpirationTime { get; set; }
}
