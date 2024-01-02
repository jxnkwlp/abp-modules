using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.FileManagement.Files;
using Passingwind.Abp.FileManagement.Localization;
using Passingwind.Abp.FileManagement.Permissions;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Entities;

namespace Passingwind.Abp.FileManagement;

public abstract class FileManagementAppService : ApplicationService
{
    protected FileManagementAppService()
    {
        LocalizationResource = typeof(FileManagementResource);
        ObjectMapperContext = typeof(FileManagementApplicationModule);
    }

    /// <summary>
    ///  Check current identity can access this container
    /// </summary>
    /// <param name="container"></param>
    /// <param name="write"></param>
    protected virtual async Task<bool> CanAccessContainerAsync(FileContainer container, bool write = false)
    {
        bool isGranted = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainers.Default);

        if (isGranted)
        {
            return true;
        }

        if (container.AccessMode == FileAccessMode.Anonymous)
        {
            return true;
        }
        else if (container.AccessMode == FileAccessMode.AnonymousReadonly && !write)
        {
            return true;
        }
        else if (container.AccessMode == FileAccessMode.Authorized && CurrentUser.IsAuthenticated)
        {
            return true;
        }
        else
        {
            // private & owner
            return container.CreatorId == CurrentUser.Id;
        }
    }

    /// <summary>
    ///  Check current identity can access this container
    /// </summary>
    /// <param name="fileContainerRepository"></param>
    /// <param name="containerName"></param>
    /// <param name="policyName"></param>
    /// <param name="write"></param>
    /// <exception cref="AbpAuthorizationException"></exception>
    protected virtual async Task CheckContainerPermissionAsync(IFileContainerRepository fileContainerRepository, string containerName, string? policyName = null, bool write = false)
    {
        var container = await fileContainerRepository.GetByNameAsync(containerName);

        if (!await CanAccessContainerAsync(container, write))
        {
            throw new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenPolicyHasNotGrantedWithPolicyName)
                 .WithData("PolicyName", policyName ?? FileManagementPermissions.FileContainers.Default);
        }
    }

    /// <summary>
    ///  Check current identity can access this container
    /// </summary>
    /// <param name="container"></param>
    /// <param name="policyName"></param>
    /// <param name="write"></param>
    /// <exception cref="AbpAuthorizationException"></exception>
    protected virtual async Task CheckContainerPermissionAsync(FileContainer container, string? policyName = null, bool write = false)
    {
        if (!await CanAccessContainerAsync(container, write))
        {
            throw new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenPolicyHasNotGrantedWithPolicyName)
                 .WithData("PolicyName", policyName ?? FileManagementPermissions.FileContainers.Default);
        }
    }

    protected virtual Task CheckFileIsInContainerAsync(FileContainer container, FileItem file)
    {
        if (file.IsDirectory)
        {
            // file is not file.
            throw new EntityNotFoundException();
        }

        return container.Id != file.ContainerId ? throw new EntityNotFoundException() : Task.CompletedTask;
    }
}
