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
    private readonly FileContainerManager _fileContainerManager;
    private readonly IFileContainerRepository _fileContainerRepository;
    private readonly FileManagementOptions _options;

    public FileContainerAppService(
        FileContainerManager fileContainerManager,
        IFileContainerRepository fileContainerRepository,
        IOptions<FileManagementOptions> options)
    {
        _fileContainerManager = fileContainerManager;
        _fileContainerRepository = fileContainerRepository;
        _options = options.Value;
    }

    public virtual async Task<PagedResultDto<FileContainerDto>> GetListAsync(FileContainerListRequestDto input)
    {
        // permission: anyone that get permission can list all or list owners.

        bool isManager = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Default);

        Guid? userId = isManager ? null : CurrentUser.Id;

        var count = await _fileContainerRepository.GetCountAsync(input.Filter, userId: userId);
        var list = await _fileContainerRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Filter, userId, nameof(FileContainer.Name));

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

        var entity = await _fileContainerRepository.GetAsync(id);

        if (!isManager && CurrentUser.Id != entity.CreatorId)
            throw new EntityNotFoundException();

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    [AllowAnonymous]
    public virtual async Task<FileContainerDto> GetByNameAsync(string name)
    {
        // permission: anyone can query container name.

        bool isManager = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Default);

        var entity = await _fileContainerRepository.GetAsync(x => x.Name == name);

        if (!isManager && CurrentUser.Id != entity.CreatorId)
            throw new EntityNotFoundException();

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task<FileContainerDto> CreateAsync(FileContainerCreateDto input)
    {
        // permission: anyone that authorized can create.

        var entity = await _fileContainerManager.CreateAsync(
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

        if (await _fileContainerManager.IsExistsAsync(entity))
        {
            throw new BusinessException(FileManagementErrorCodes.ContainerExist).WithData("name", entity.Name);
        }

        await _fileContainerRepository.InsertAsync(entity);

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task<FileContainerDto> UpdateAsync(Guid id, FileContainerUpdateDto input)
    {
        // permission: anyone that get permission or owners can update.

        bool isManager = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Update);

        var entity = await _fileContainerRepository.GetAsync(id);

        if (!isManager && CurrentUser.Id != entity.CreatorId)
            throw new EntityNotFoundException();

        entity.Description = input.Description;
        entity.MaximumEachFileSize = input.MaximumEachFileSize ?? 0;
        entity.MaximumFileQuantity = input.MaximumFileQuantity ?? 0;
        entity.OverrideBehavior = input.OverrideBehavior ?? FileOverrideBehavior.None;
        entity.AllowAnyFileExtension = input.AllowAnyFileExtension ?? false;
        entity.AllowedFileExtensions = input.AllowedFileExtensions;
        entity.ProhibitedFileExtensions = input.ProhibitedFileExtensions;
        entity.AutoDeleteBlob = input.AutoDeleteBlob;

        entity.SetAccessMode(input.AccessMode ?? _options.DefaultContainerAccessMode);

        input.MapExtraPropertiesTo(entity);

        await _fileContainerRepository.UpdateAsync(entity);

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        // permission: anyone that get permission or owners can delete.

        bool isManager = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Delete);

        var entity = await _fileContainerRepository.GetAsync(id);

        if (!isManager && CurrentUser.Id != entity.CreatorId)
            return;

        await _fileContainerRepository.DeleteAsync(id);
    }
}
