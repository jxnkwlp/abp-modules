using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Account;

public class UserBasicDto : EntityDto<Guid>
{
    public string UserName { get; set; } = null!;
    public string? Name { get; set; }
    public string? SurName { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
}
