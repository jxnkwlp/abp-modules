﻿using System;

namespace Passingwind.Abp.FileManagement;

public class FileMoveAdminRequestDto
{
    public string? NewFileName { get; set; }
    public Guid? TargetParentId { get; set; }
    public bool Override { get; set; }
}
