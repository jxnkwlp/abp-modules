namespace Passingwind.Abp.FileManagement.Files;

public enum FileAccessMode
{
    /// <summary>
    ///  Anonymous can read & upload
    /// </summary>
    Anonymous = 0,
    /// <summary>
    ///  Anonymous can read
    /// </summary>
    Readonly = 1,
    /// <summary>
    ///  Only authorized user can read & upload
    /// </summary>
    Authorized = 10,
    /// <summary>
    ///  Only created user can read & upload
    /// </summary>
    Private = 20,
}
