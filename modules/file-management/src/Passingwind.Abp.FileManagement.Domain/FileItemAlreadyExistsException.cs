using System;

namespace Passingwind.Abp.FileManagement;

public class FileItemAlreadyExistsException : Exception
{
    public FileItemAlreadyExistsException()
    {
    }

    public FileItemAlreadyExistsException(string message) : base(message)
    {
    }

    public FileItemAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
