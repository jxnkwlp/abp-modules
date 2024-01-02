using System;

namespace Passingwind.Abp.FileManagement.Files;

public class FileMoveAdminRequestDto
{
    public Guid? TargetId { get; set; }
    public Guid? TargetContainerId { get; set; }
    public bool Override { get; set; }
}
