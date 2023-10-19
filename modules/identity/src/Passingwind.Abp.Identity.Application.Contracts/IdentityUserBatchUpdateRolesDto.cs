namespace Passingwind.Abp.Identity;

public class IdentityUserBatchUpdateRolesDto : IdentityUserBatchInputDto
{
    public bool Override { get; set; }
    public string[]? RoleNames { get; set; }
}
