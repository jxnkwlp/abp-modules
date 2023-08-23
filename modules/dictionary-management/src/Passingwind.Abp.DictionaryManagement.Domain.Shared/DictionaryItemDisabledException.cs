using System;
using System.Runtime.Serialization;
using Volo.Abp.ExceptionHandling;

namespace Passingwind.Abp.DictionaryManagement;

public class DictionaryItemDisabledException : Exception, IHasHttpStatusCode, IHasErrorCode
{
    public DictionaryItemDisabledException()
    {
    }

    public DictionaryItemDisabledException(string? message) : base(message)
    {
    }

    public DictionaryItemDisabledException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DictionaryItemDisabledException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public int HttpStatusCode => 404;

    public string Code => DictionaryManagementErrorCodes.ItemNotFound;
}
