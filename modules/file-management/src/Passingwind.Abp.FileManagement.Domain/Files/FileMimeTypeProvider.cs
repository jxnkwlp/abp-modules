using System;
using System.IO;
using HeyRed.Mime;

namespace Passingwind.Abp.FileManagement.Files;

public class FileMimeTypeProvider : IFileMimeTypeProvider
{
    public string Get(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
        }

        return MimeTypesMap.GetMimeType(Path.GetExtension(fileName));
    }
}