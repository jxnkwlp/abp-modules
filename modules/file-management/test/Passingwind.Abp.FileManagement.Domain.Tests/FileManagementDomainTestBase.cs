﻿namespace Passingwind.Abp.FileManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class FileManagementDomainTestBase : FileManagementTestBase<FileManagementDomainTestModule>
{
    protected string TestContainerName = "test-01";
}
