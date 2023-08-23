namespace Passingwind.Abp.DynamicPermissionManagement.Options;

public class DynamicPermissionManagementOptions
{
    /// <summary>
    ///  Default: 'dy:'
    /// </summary>
    public string PermissionNamePrefix { get; set; } = "dy:";
    /// <summary>
    ///  Default: true
    /// </summary>
    public bool AutoCleanPermissions { get; set; } = true;
}
