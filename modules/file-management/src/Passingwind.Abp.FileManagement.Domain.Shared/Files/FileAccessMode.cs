namespace Passingwind.Abp.FileManagement.Files;

public enum FileAccessMode
{
    /// <summary>
    ///  Anonymous can read & write
    /// </summary>
    Anonymous = 0,
    /// <summary>
    ///  Anonymous can read, authorized user can write
    /// </summary>
    AnonymousReadonly = 1,
    /// <summary>
    ///  Only authorized user can read & write
    /// </summary>
    Authorized = 10,
    /// <summary>
    ///  control by <see cref="FileContainerAccess"/>
    /// </summary>
    Private = 20,

    ///// <summary>
    /////  Readonly
    ///// </summary>
    //Readonly = 30,
}
