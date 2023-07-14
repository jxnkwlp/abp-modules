namespace Passingwind.Abp.FileManagement.Files;

public enum FileAccessMode
{
    /// <summary>
    ///  Anonymous to read & write
    /// </summary>
    Public = 0,
    /// <summary>
    ///  Only create user can read & write
    /// </summary>
    Private,
    /// <summary>
    ///  Only authorized user can read & write
    /// </summary>
    Authorize,
}
