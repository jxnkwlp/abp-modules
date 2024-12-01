using System;

namespace Passingwind.Abp.FileManagement;

public class FileShareAdminListRequestDto : FileShareListRequestDto
{
    public Guid? ContainerId { get; set; }
    public Guid? UserId { get; set; }
}
