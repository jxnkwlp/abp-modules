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
        bool isGranted = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Default);

        Guid? userId = isGranted ? null : CurrentUser.Id;

        var count = await _fileContainerRepository.GetCountAsync(input.Filter, userId: userId);
        var list = await _fileContainerRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Filter, userId, nameof(FileContainer.CreationTime) + " desc");

        return new PagedResultDto<FileContainerDto>()
        {
            Items = ObjectMapper.Map<List<FileContainer>, List<FileContainerDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<FileContainerDto> GetAsync(Guid id)
    {
        bool isGranted = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Default);

        var entity = await _fileContainerRepository.GetAsync(id);

        if (!isGranted && CurrentUser.Id != entity.CreatorId)
            throw new EntityNotFoundException();

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task<FileContainerDto> GetByNameAsync(string name)
    {
        bool isGranted = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Default);

        var entity = await _fileContainerRepository.GetAsync(x => x.Name == name);

        if (!isGranted && CurrentUser.Id != entity.CreatorId)
            throw new EntityNotFoundException();

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task<FileContainerDto> CreateAsync(FileContainerCreateDto input)
    {
        var entity = await _fileContainerManager.CreateAsync(
            input.Name,
            input.AccessMode,
            input.Description,
            input.MaximumEachFileSize,
            input.MaximumFileQuantity,
            input.OverrideBehavior,
            input.AllowAnyFileExtension,
            input.AllowedFileExtensions,
            input.ProhibitedFileExtensions);

        input.MapExtraPropertiesTo(entity);

        if (await _fileContainerManager.IsExistsAsync(entity))
        {
            throw new BusinessException(FileManagementErrorCodes.FileContainerExist).WithData("name", entity.Name);
        }

        await _fileContainerRepository.InsertAsync(entity);

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task<FileContainerDto> UpdateAsync(Guid id, FileContainerUpdateDto input)
    {
        bool isGranted = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Update);

        var entity = await _fileContainerRepository.GetAsync(id);

        if (!isGranted && CurrentUser.Id != entity.CreatorId)
            throw new EntityNotFoundException();

        entity.Description = input.Description;
        entity.MaximumEachFileSize = input.MaximumEachFileSize ?? 0;
        entity.MaximumFileQuantity = input.MaximumFileQuantity ?? 0;
        entity.OverrideBehavior = input.OverrideBehavior ?? FileOverrideBehavior.None;
        entity.AllowAnyFileExtension = input.AllowAnyFileExtension ?? false;
        entity.AllowedFileExtensions = input.AllowedFileExtensions;
        entity.ProhibitedFileExtensions = input.ProhibitedFileExtensions;

        entity.SetAccessMode(input.AccessMode ?? _options.DefaultContainerAccessMode);

        input.MapExtraPropertiesTo(entity);

        if (await _fileContainerManager.IsExistsAsync(entity))
        {
            throw new BusinessException(FileManagementErrorCodes.FileContainerExist).WithData("name", entity.Name);
        }

        await _fileContainerRepository.UpdateAsync(entity);

        return ObjectMapper.Map<FileContainer, FileContainerDto>(entity);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        bool isGranted = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Delete);

        var entity = await _fileContainerRepository.GetAsync(id);

        if (!isGranted && CurrentUser.Id != entity.CreatorId)
            return;

        await _fileContainerRepository.DeleteAsync(id);
    }

}
