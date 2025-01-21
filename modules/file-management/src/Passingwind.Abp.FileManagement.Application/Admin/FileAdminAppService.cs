using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Options;
using Passingwind.Abp.FileManagement.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement.Admin;

[Authorize(FileManagementPermissions.Files.Default)]
public class FileAdminAppService : FileManagementAppService, IFileAdminAppService
{
    protected FileManagementOptions FileManagementOptions { get; }
    protected IFileItemManager FileManager { get; }
    protected IFileItemRepository FileRepository { get; }
    protected IFileContainerRepository FileContainerRepository { get; }
    protected IFileMimeTypeProvider FileMimeTypeProvider { get; }

    public FileAdminAppService(
        IOptions<FileManagementOptions> fileManagementOptions,
        IFileItemManager fileManager,
        IFileItemRepository fileRepository,
        IFileContainerRepository fileContainerRepository,
        IFileMimeTypeProvider fileMimeTypeProvider)
    {
        FileManagementOptions = fileManagementOptions.Value;
        FileManager = fileManager;
        FileRepository = fileRepository;
        FileContainerRepository = fileContainerRepository;
        FileMimeTypeProvider = fileMimeTypeProvider;
    }

    public virtual async Task<PagedResultDto<FileItemDto>> GetListAsync(Guid containerId, FilePagedListRequestDto input)
    {
        var count = await FileRepository.GetCountAsync(
            filter: input.Filter,
            containerId: containerId,
            parentId: input.ParentId ?? Guid.Empty,
            isDirectory: input.IsDirectory);
        var list = await FileRepository.GetPagedListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Filter,
            containerId: containerId,
            parentId: input.ParentId ?? Guid.Empty,
            isDirectory: input.IsDirectory,
            sorting: input.Sorting ?? $"{nameof(FileItem.IsDirectory)} desc,{nameof(FileItem.FileName)}");

        return new PagedResultDto<FileItemDto>()
        {
            Items = ObjectMapper.Map<List<FileItem>, List<FileItemDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<FileItemDto> GetAsync(Guid containerId, Guid id)
    {
        var entity = await FileRepository.GetAsync(id);

        return containerId != entity.ContainerId ? throw new EntityNotFoundException() : ObjectMapper.Map<FileItem, FileItemDto>(entity);
    }

    [Authorize(FileManagementPermissions.Files.Delete)]
    public virtual async Task DeleteAsync(Guid containerId, Guid id)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        await FileManager.DeleteAsync(id);
    }

    [Authorize(FileManagementPermissions.Files.Download)]
    public virtual async Task<IRemoteStreamContent?> GetBlobAsync(Guid containerId, Guid id)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var entity = await FileRepository.GetAsync(id);

        if (containerId != entity.ContainerId || entity.IsDirectory)
        {
            throw new EntityNotFoundException();
        }

        var fileStream = await FileManager.GetFileSteamAsync(id);

        return fileStream == null
            ? throw new BlobNotFoundException()
            : new RemoteStreamContent(fileStream, entity.FileName, entity.MimeType);
    }

    [Authorize(FileManagementPermissions.Files.Download)]
    public virtual async Task<Stream?> GeBlobStreamAsync(Guid containerId, Guid id)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var entity = await FileRepository.GetAsync(id);

        return containerId != entity.ContainerId || entity.IsDirectory
            ? throw new EntityNotFoundException()
            : await FileManager.GetFileSteamAsync(id);
    }

    [Authorize(FileManagementPermissions.Files.Download)]
    public virtual async Task<byte[]?> GetBlobBytesAsync(Guid containerId, Guid id)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var entity = await FileRepository.GetAsync(id);

        return containerId != entity.ContainerId || entity.IsDirectory
            ? throw new EntityNotFoundException()
            : await FileManager.GetFileBytesAsync(id);
    }

    [Authorize(FileManagementPermissions.Files.Upload)]
    public virtual async Task<FileItemDto> CreateAsync(Guid containerId, FileCreateDto input)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var fileBytes = await input.File.GetStream().GetAllBytesAsync();

        var entity = await CreateFileAsync(container, fileName: input.File.FileName!, parentId: input.ParentId ?? Guid.Empty, mimeType: null, fileBytes: fileBytes, overExists: input.Override, extensibleObject: input);

        return ObjectMapper.Map<FileItem, FileItemDto>(entity);
    }

    [Authorize(FileManagementPermissions.Files.Upload)]
    public virtual async Task<FileItemDto> CreateByStreamAsync(Guid containerId, FileCreateByStreamDto input)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var bytes = await input.FileStream.GetAllBytesAsync();

        var entity = await CreateFileAsync(container, fileName: input.FileName, parentId: input.ParentId ?? Guid.Empty, mimeType: input.MimeType, fileBytes: bytes, overExists: input.Override, extensibleObject: input);

        return ObjectMapper.Map<FileItem, FileItemDto>(entity);
    }

    [Authorize(FileManagementPermissions.Files.Upload)]
    public virtual async Task<FileItemDto> CreateByBytesAsync(Guid containerId, FileCreateByBytesDto input)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var bytes = input.FileData;

        var entity = await CreateFileAsync(container, fileName: input.FileName, parentId: input.ParentId ?? Guid.Empty, mimeType: input.MimeType, fileBytes: bytes, overExists: input.Override, extensibleObject: input);

        return ObjectMapper.Map<FileItem, FileItemDto>(entity);
    }

    [Authorize(FileManagementPermissions.Files.Update)]
    public virtual async Task<FileItemDto> MoveAsync(Guid containerId, Guid id, FileMoveAdminRequestDto input)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        await FileManager.MoveAsync(fileId: id, targetFileName: input.NewFileName ?? entity.FileName, targetParentId: input.TargetParentId);

        return ObjectMapper.Map<FileItem, FileItemDto>(await FileRepository.GetAsync(id));
    }

    [Authorize(FileManagementPermissions.Files.Update)]
    public virtual async Task<FileItemDto> UpdateAsync(Guid containerId, Guid id, FileUpdateDto input)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var entity = await FileRepository.GetAsync(id);

        if (entity.FileName != input.FileName)
        {
            await FileManager.ChangeNameAsync(id, input.FileName);
        }

        if (input.Tags != null)
        {
            await FileManager.AddTagsAsync(id, input.Tags);
        }

        input.MapExtraPropertiesTo(entity);
        await FileRepository.UpdateAsync(entity);

        return ObjectMapper.Map<FileItem, FileItemDto>(await FileRepository.GetAsync(id));
    }

    public virtual async Task<FileItemDto> CreateDirectoryAsync(Guid containerId, FileDirectoryCreateDto input)
    {
        await FileContainerRepository.GetAsync(containerId);

        var exists = await FileManager.IsDirectoryExistsAsync(containerId, input.FileName, parentId: input.ParentId);
        if (!input.Force && exists)
        {
            throw new BusinessException(FileManagementErrorCodes.FileExists).WithData("fileName", input.FileName);
        }

        FileItem entity;
        if (exists)
        {
            entity = await FileRepository.GetByNameAsync(containerId, input.FileName, parentId: input.ParentId, isDirectory: true);
        }
        else
        {
            entity = await FileManager.CreateDirectoryAsync(containerId, input.FileName, parentId: input.ParentId);
            await CurrentUnitOfWork!.SaveChangesAsync();
        }

        input.MapExtraPropertiesTo(entity);

        entity = await FileRepository.UpdateAsync(entity!);

        return ObjectMapper.Map<FileItem, FileItemDto>(entity);
    }

    protected virtual async Task<FileItem> CreateFileAsync(FileContainer container, string fileName, Guid parentId, string? mimeType, byte[] fileBytes, bool overExists = false, ExtensibleObject? extensibleObject = null)
    {
        mimeType ??= FileMimeTypeProvider.Get(fileName);

        var entity = await FileManager.SaveAsync(containerId: container.Id, fileName: fileName, bytes: fileBytes, mimeType: mimeType, parentId: parentId, overrideExisting: overExists);

        await CurrentUnitOfWork!.SaveChangesAsync();

        extensibleObject?.MapExtraPropertiesTo(entity);

        await FileRepository.UpdateAsync(entity);

        return entity;
    }

    public async Task SetTagsAsync(Guid containerId, Guid id, FileTagsUpdateDto input)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        await FileManager.SetTagsAsync(id, input.Tags);
    }
}
