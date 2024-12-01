using System;
using Volo.Abp.Domain.Entities;

namespace Passingwind.Abp.FileManagement;

public class FilePath : Entity
{
    public FilePath(Guid fileId, string fullPath)
    {
        FileId = fileId;
        FullPath = fullPath;
    }

    protected FilePath()
    {
    }

    public Guid FileId { get; protected set; }

    public string FullPath { get; protected set; } = null!;

    public override object?[] GetKeys()
    {
        return new object[] { FileId };
    }
}
