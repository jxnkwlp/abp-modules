using HeyRed.Mime;
using System;
using System.IO;

namespace Passingwind.Abp.FileManagement;

public class FileMimeTypeProvider : IFileMimeTypeProvider
{
    public string Get(string fileName)
    {
        return string.IsNullOrWhiteSpace(fileName)
            ? throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName))
            : MimeTypesMap.GetMimeType(Path.GetExtension(fileName));
    }
}