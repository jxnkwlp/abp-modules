namespace Passingwind.Abp.FileManagement;

/// <summary>
///  File upload override behavior when file exists
/// </summary>
public enum FileOverrideBehavior
{
    None = 0,
    Override = 1,
    Rename = 2,
}
