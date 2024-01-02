using System;

namespace Passingwind.Abp.FileManagement.Files;

public class FileMoveRequestDto
{
    public Guid? TargetId { get; set; }
    public bool Override { get; set; }
}
