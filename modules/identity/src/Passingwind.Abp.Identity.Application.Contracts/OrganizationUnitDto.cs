using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Identity;

public class OrganizationUnitDto : ExtensibleFullAuditedEntityDto<Guid>
{
    public virtual Guid? ParentId { get; set; }

    public virtual string Code { get; set; } = null!;

    public virtual string DisplayName { get; set; } = null!;

    public Guid[] RoleIds { get; set; }

    public OrganizationUnitDto()
    {
        RoleIds = new Guid[0];
    }
}
