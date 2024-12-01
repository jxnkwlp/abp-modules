using System.IO;
using MimeMapping;

namespace Passingwind.Abp.FileManagement;

public class FileMimeTypeProvider : IFileMimeTypeProvider
{
    public string Get(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return string.Empty;

        if (string.IsNullOrWhiteSpace(Path.GetExtension(fileName)))
            return string.Empty;

        return MimeUtility.GetMimeMapping(Path.GetExtension(fileName));
    }
}
