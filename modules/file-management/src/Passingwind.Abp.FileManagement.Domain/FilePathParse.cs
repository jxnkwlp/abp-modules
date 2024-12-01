using Volo.Abp;

namespace Passingwind.Abp.FileManagement;

public readonly struct FilePathParse
{
    /// <summary>
    ///  The container name
    /// </summary>
    public string Container { get; }

    /// <summary>
    ///  The full path without container
    /// </summary>
    public string FullName { get; }

    /// <summary>
    ///  The name
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///  The full path without container
    /// </summary>
    public string DirectoryPath { get; }
    public bool IsDirectory { get; }

    /// <summary>
    ///  Get directory path with container
    /// </summary>
    public string GetDirectory() => $"/{Container}{DirectoryPath}/";
    /// <summary>
    ///  Get full path with container
    /// </summary>
    public string GetFullName() => $"/{Container}{FullName}";

    /// <summary>
    ///  format: <![CDATA[/<container>/<file-path-1>/<file-path-2>/<file-path-3>]]>
    /// </summary>
    public FilePathParse(string fileFullPath, bool isDirectory = false)
    {
        IsDirectory = isDirectory;
        fileFullPath = fileFullPath.Trim('/');
        CheckFullPath(fileFullPath);
        Container = GetContainerName(fileFullPath);
        var path = GetFilePath(fileFullPath);
        if (path.LastIndexOf('/') > 0)
        {
            DirectoryPath = path.Substring(0, path.LastIndexOf("/"));
            Name = path.Substring(path.LastIndexOf("/") + 1);
        }
        else
        {
            DirectoryPath = string.Empty;
            Name = path.Trim('/');
        }
        FullName = path + (IsDirectory ? "/" : "");
    }

    public static FilePathParse FromFilePath(string fileFullPath) => new FilePathParse(fileFullPath);
    public static FilePathParse FromDirectoryPath(string fileFullPath) => new FilePathParse(fileFullPath, true);

    private static void CheckFullPath(string fullPath)
    {
        if (fullPath.IndexOf("/") <= 0)
            throw new UserFriendlyException("The file path must need start '/' with container name");
    }

    private static string GetContainerName(string fullPath)
    {
        return fullPath.Split('/')[0];
    }

    private static string GetFilePath(string fullPath)
    {
        return fullPath.Substring(fullPath.IndexOf('/', 1)).TrimEnd('/');
    }
}
