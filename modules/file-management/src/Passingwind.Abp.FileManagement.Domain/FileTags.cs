using System;
using Volo.Abp.Domain.Entities;

namespace Passingwind.Abp.FileManagement;

public class FileTags : Entity
{
    public Guid FileId { get; set; }
    public string Name { get; set; } = null!;

    public override object[] GetKeys()
    {
        return [FileId, Name];
    }
}
