﻿using System;
using Volo.Abp.ExceptionHandling;

namespace Passingwind.Abp.FileManagement;

public class BlobNotFoundException : Exception, IHasHttpStatusCode, IHasErrorCode
{
    public BlobNotFoundException()
    {
    }

    public BlobNotFoundException(string message) : base(message)
    {
    }

    public BlobNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public int HttpStatusCode => 404;

    public string? Code => FileManagementErrorCodes.BlobNotExists;
}
