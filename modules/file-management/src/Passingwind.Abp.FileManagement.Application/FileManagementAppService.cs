using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.FileManagement.Files;
using Passingwind.Abp.FileManagement.Localization;
using Passingwind.Abp.FileManagement.Permissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Entities;

namespace Passingwind.Abp.FileManagement;

public abstract class FileManagementAppService : ApplicationService
{
    protected FileManagementAppService()
    {
        LocalizationResource = typeof(FileManagementResource);
        ObjectMapperContext = typeof(PassingwindAbpFileManagementApplicationModule);
    }

    /// <summary>
    ///  Check current identity can access this container
    /// </summary>
    /// <param name="container"></param>
    /// <param name="write"></param>
    protected virtual async Task<bool> CanAccessContainerAsync(FileContainer container, bool write = false)
    {
        bool isGranted = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Default);

        if (isGranted)
            return true;

        if (container.AccessMode == FileAccessMode.Anonymous)
        {
            return true;
        }
        else if (container.AccessMode == FileAccessMode.Readonly && !write)
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
                 .WithData("PolicyName", policyName ?? FileManagementPermissions.FileContainer.Default);
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
                 .WithData("PolicyName", policyName ?? FileManagementPermissions.FileContainer.Default);
        }
    }

    protected virtual Task CheckFileIsInContainerAsync(FileContainer container, File file)
    {
        if (file.IsDirectory)
        {
            // file is not file.
            throw new EntityNotFoundException();
        }

        if (container.Id != file.ContainerId)
            throw new EntityNotFoundException();

        return Task.CompletedTask;
    }
}
