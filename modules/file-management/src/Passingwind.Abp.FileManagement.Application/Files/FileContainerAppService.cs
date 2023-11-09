using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Options;
using Passingwind.Abp.FileManagement.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement.Files;

[Authorize]
public class FileContainerAppService : FileManagementAppService, IFileContainerAppService
{
    protected FileManagementOptions FileManagementOptions { get; }
    protected FileContainerManager FileContainerManager { get; }
    protected IFileContainerRepository FileContainerRepository { get; }
    protected IFileRepository FileRepository { get; }
    protected IFileManager FileManager { get; }

    public FileContainerAppService(
        IOptions<FileManagementOptions> fileManagementOptions,
        FileContainerManager fileContainerManager,
        IFileContainerRepository fileContainerRepository,
        IFileRepository fileRepository,
        IFileManager fileManager)
    {
        FileManagementOptions = fileManagementOptions.Value;
        FileContainerManager = fileContainerManager;
        FileContainerRepository = fileContainerRepository;
        FileRepository = fileRepository;
        FileManager = fileManager;
    }

    public virtual async Task<ListResultDto<FileContainerDto>> GetAllListAsync()
    {
        // permission: anyone that get permission can list all or list owners.

        bool isManager = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Default);

        Guid? userId = isManager ? null : CurrentUser.Id;

        List<FileContainer> list = await FileContainerRepository.GetListAsync(userId: userId);

        return new ListResultDto<FileContainerDto>(ObjectMapper.Map<List<FileContainer>, List<FileContainerDto>>(list));
    }

    public virtual async Task<PagedResultDto<FileContainerDto>> GetListAsync(FileContainerListRequestDto input)
    {
        // permission: anyone that get permission can list all or list owners.

        bool isManager = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Default);

        Guid? userId = isManager ? null : CurrentUser.Id;

        long count = await FileContainerRepository.GetCountAsync(input.Filter, userId: userId);
        List<FileContainer> list = await FileContainerRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Filter, userId, nameof(FileContainer.Name));

        return new PagedResultDto<FileContainerDto>()
        {
            Items = ObjectMapper.Map<List<FileContainer>, List<FileContainerDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<FileContainerDto> GetAsync(Guid id)
    {
        // permission: anyone that get permission or owner can read.

        bool isManager = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Default);

        FileContainer entity = await FileContainerRepository.GetAsync(id);

        return !isManager && CurrentUser.Id != entity.CreatorId
            ? throw new EntityNotFoundException()
            : ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    [AllowAnonymous]
    public virtual async Task<FileContainerDto> GetByNameAsync(string name)
    {
        // permission: anyone can query container name.

        bool isManager = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Default);

        FileContainer entity = await FileContainerRepository.GetAsync(x => x.Name == name);

        return !isManager && CurrentUser.Id != entity.CreatorId
            ? throw new EntityNotFoundException()
            : ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task<FileContainerDto> CreateAsync(FileContainerCreateDto input)
    {
        // permission: anyone that authorized can create.

        FileContainer entity = await FileContainerManager.CreateAsync(
            input.Name,
            input.AccessMode,
            input.Description,
            input.MaximumEachFileSize,
            input.MaximumFileQuantity,
            input.OverrideBehavior,
            input.AllowAnyFileExtension,
            input.AllowedFileExtensions,
            input.ProhibitedFileExtensions,
            input.AutoDeleteBlob);

        input.MapExtraPropertiesTo(entity);

        if (await FileContainerManager.IsExistsAsync(entity))
        {
            throw new BusinessException(FileManagementErrorCodes.ContainerExist).WithData("name", entity.Name);
        }

        _ = await FileContainerRepository.InsertAsync(entity);

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task<FileContainerDto> UpdateAsync(Guid id, FileContainerUpdateDto input)
    {
        // permission: anyone that get permission or owners can update.

        bool isManager = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Update);

        FileContainer entity = await FileContainerRepository.GetAsync(id);

        if (!isManager && CurrentUser.Id != entity.CreatorId)
        {
            throw new EntityNotFoundException();
        }

        entity.Description = input.Description;
        entity.MaximumEachFileSize = input.MaximumEachFileSize ?? 0;
        entity.MaximumFileQuantity = input.MaximumFileQuantity ?? 0;
        entity.OverrideBehavior = input.OverrideBehavior ?? FileOverrideBehavior.None;
        entity.AllowAnyFileExtension = input.AllowAnyFileExtension ?? false;
        entity.AllowedFileExtensions = input.AllowedFileExtensions;
        entity.ProhibitedFileExtensions = input.ProhibitedFileExtensions;
        entity.AutoDeleteBlob = input.AutoDeleteBlob;

        entity.SetAccessMode(input.AccessMode ?? FileManagementOptions.DefaultContainerAccessMode);

        input.MapExtraPropertiesTo(entity);

        _ = await FileContainerRepository.UpdateAsync(entity);

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        // permission: anyone that get permission or owners can delete.

        bool isManager = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Delete);

        FileContainer entity = await FileContainerRepository.GetAsync(id);

        if (!isManager && CurrentUser.Id != entity.CreatorId)
        {
            return;
        }

        long fileCount = await FileRepository.GetCountAsync(containerId: id);

        if (fileCount > 0 && !FileManagementOptions.AllowForceDeleteContainer)
        {
            throw new BusinessException(FileManagementErrorCodes.ContainerNotAllowForceDelete);
        }

        // 1. delete file
        await FileManager.ClearContainerFilesAsync(entity);

        // 2. delete container 
        await FileContainerRepository.DeleteAsync(id);
    }
}
