using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Options;
using Passingwind.Abp.FileManagement.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement.Admin;

[Authorize(FileManagementPermissions.FileContainers.Default)]
public class FileContainerAdminAppService : FileManagementAppService, IFileContainerAdminAppService
{
    protected FileManagementOptions FileManagementOptions { get; }
    protected IFileContainerManager FileContainerManager { get; }
    protected IFileContainerRepository FileContainerRepository { get; }
    protected IFileItemRepository FileRepository { get; }
    protected IFileItemManager FileManager { get; }

    public FileContainerAdminAppService(
        IOptions<FileManagementOptions> fileManagementOptions,
        IFileContainerManager fileContainerManager,
        IFileContainerRepository fileContainerRepository,
        IFileItemRepository fileRepository,
        IFileItemManager fileManager)
    {
        FileManagementOptions = fileManagementOptions.Value;
        FileContainerManager = fileContainerManager;
        FileContainerRepository = fileContainerRepository;
        FileRepository = fileRepository;
        FileManager = fileManager;
    }

    public virtual async Task<ListResultDto<FileContainerDto>> GetAllListAsync()
    {
        // return all list  
        var list = await FileContainerRepository.GetListAsync();

        return new ListResultDto<FileContainerDto>(ObjectMapper.Map<List<FileContainer>, List<FileContainerDto>>(list));
    }

    public virtual async Task<PagedResultDto<FileContainerDto>> GetListAsync(FileContainerAdminListRequestDto input)
    {
        var count = await FileContainerRepository.GetCountAsync(input.Filter);
        var list = await FileContainerRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Filter, sorting: nameof(FileContainer.Name));

        return new PagedResultDto<FileContainerDto>()
        {
            Items = ObjectMapper.Map<List<FileContainer>, List<FileContainerDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<FileContainerDto> GetAsync(Guid id)
    {
        var entity = await FileContainerRepository.GetAsync(id);

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task<FileContainerDto> CreateAsync(FileContainerAdminCreateDto input)
    {
        var entity = await FileContainerManager.CreateAsync(
            name: input.Name,
            accessMode: input.AccessMode ?? FileManagementOptions.DefaultContainerAccessMode,
            overrideBehavior: input.OverrideBehavior ?? FileManagementOptions.DefaultOverrideBehavior,
            description: input.Description,
            maximumEachFileSize: input.MaximumEachFileSize,
            maximumFileQuantity: input.MaximumFileQuantity,
            allowAnyFileExtension: input.AllowAnyFileExtension,
            allowedFileExtensions: input.AllowedFileExtensions,
            prohibitedFileExtensions: input.ProhibitedFileExtensions,
            autoDeleteBlob: input.AutoDeleteBlob);

        input.MapExtraPropertiesTo(entity);

        if (await FileContainerManager.IsExistsAsync(entity))
        {
            throw new BusinessException(FileManagementErrorCodes.ContainerExist).WithData("name", entity.Name);
        }

        entity = await FileContainerRepository.InsertAsync(entity);

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task<FileContainerDto> UpdateAsync(Guid id, FileContainerAdminUpdateDto input)
    {
        var entity = await FileContainerRepository.GetAsync(id);

        entity.Description = input.Description;
        entity.MaximumEachFileSize = input.MaximumEachFileSize ?? 0;
        entity.MaximumFileQuantity = input.MaximumFileQuantity ?? 0;
        entity.OverrideBehavior = input.OverrideBehavior ?? FileOverrideBehavior.None;
        entity.AllowAnyFileExtension = input.AllowAnyFileExtension ?? false;
        entity.AllowedFileExtensions = input.AllowedFileExtensions;
        entity.ProhibitedFileExtensions = input.ProhibitedFileExtensions;
        entity.AutoDeleteBlob = input.AutoDeleteBlob;

        entity.ConcurrencyStamp = input.ConcurrencyStamp;

        entity.SetAccessMode(input.AccessMode ?? FileManagementOptions.DefaultContainerAccessMode);

        input.MapExtraPropertiesTo(entity);

        entity = await FileContainerRepository.UpdateAsync(entity);

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await FileContainerRepository.GetAsync(id);

        var fileCount = await FileRepository.GetCountAsync(containerId: id);

        if (fileCount > 0 && !FileManagementOptions.AllowForceDeleteContainer)
        {
            throw new BusinessException(FileManagementErrorCodes.ContainerNotAllowForceDelete);
        }

        // 1. delete file
        await FileManager.ClearContainerFilesAsync(entity.Id);

        // 2. delete container 
        await FileContainerRepository.DeleteAsync(id);
    }
}
