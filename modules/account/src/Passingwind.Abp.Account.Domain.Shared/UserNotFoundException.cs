using System;
using Volo.Abp.ExceptionHandling;

namespace Passingwind.Abp.Account;

public class UserNotFoundException : Exception, IHasErrorCode, IHasHttpStatusCode
{
    public UserNotFoundException()
    {
    }

    public UserNotFoundException(string? message) : base(message)
    {
    }

    public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public string Code => AccountErrorCodes.UserNotFound;
    public int HttpStatusCode => 404;
}
