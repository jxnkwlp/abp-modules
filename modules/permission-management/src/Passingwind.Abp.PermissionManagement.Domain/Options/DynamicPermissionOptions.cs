namespace Passingwind.Abp.PermissionManagement.Options;

public class DynamicPermissionOptions
{
    /// <summary>
    ///  Default: 'dym:'
    /// </summary>
    public string PermissionNamePrefix { get; set; } = "dym:";
    /// <summary>
    ///  Default: true
    /// </summary>
    public bool AutoCleanPermissions { get; set; } = true;
}
