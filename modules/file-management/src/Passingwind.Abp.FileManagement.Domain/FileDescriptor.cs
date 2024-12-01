using System;
using System.IO;

namespace Passingwind.Abp.FileManagement;

public class FileDescriptor
{
    public string FileName { get; set; } = null!;
    public Stream Stream { get; set; } = null!;
    public DateTime Created { get; set; }
}
