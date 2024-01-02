using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Files;
using Passingwind.Abp.FileManagement.Options;
using Passingwind.Abp.FileManagement.Permissions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement;

[Authorize(FileManagementPermissions.Files.Default)]
public class FileAdminAppService : FileManagementAppService, IFileAdminAppService
{
    protected FileManagementOptions FileManagementOptions { get; }
    protected IFileManager FileManager { get; }
    protected IFileRepository FileRepository { get; }
    protected IFileContainerRepository FileContainerRepository { get; }
    protected IFileContainerManager FileContainerManager { get; }
    protected IFileMimeTypeProvider FileMimeTypeProvider { get; }

    public FileAdminAppService(
        IOptions<FileManagementOptions> fileManagementOptions,
        IFileManager fileManager,
        IFileRepository fileRepository,
        IFileContainerRepository fileContainerRepository,
        IFileContainerManager fileContainerManager,
        IFileMimeTypeProvider fileMimeTypeProvider)
    {
        FileManagementOptions = fileManagementOptions.Value;
        FileManager = fileManager;
        FileRepository = fileRepository;
        FileContainerRepository = fileContainerRepository;
        FileContainerManager = fileContainerManager;
        FileMimeTypeProvider = fileMimeTypeProvider;
    }

    public virtual async Task<PagedResultDto<FileDto>> GetListAsync(Guid containerId, FilePagedListRequestDto input)
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

        return new PagedResultDto<FileDto>()
        {
            Items = ObjectMapper.Map<List<FileItem>, List<FileDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<FileDto> GetAsync(Guid containerId, Guid id)
    {
        var entity = await FileRepository.GetAsync(id);

        return containerId != entity.ContainerId ? throw new EntityNotFoundException() : ObjectMapper.Map<FileItem, FileDto>(entity);
    }

    [Authorize(FileManagementPermissions.Files.Delete)]
    public virtual async Task DeleteAsync(Guid containerId, Guid id)
    {
        var entity = await FileRepository.GetAsync(id);

        if (entity.IsDirectory)
        {
            // TODO delete files
        }

        await FileRepository.DeleteAsync(entity);
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

        var fileStream = await FileManager.GetFileSteamAsync(container.Id, id);

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
            : await FileManager.GetFileSteamAsync(container.Id, id);
    }

    [Authorize(FileManagementPermissions.Files.Download)]
    public virtual async Task<byte[]?> GetBlobBytesAsync(Guid containerId, Guid id)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var entity = await FileRepository.GetAsync(id);

        return containerId != entity.ContainerId || entity.IsDirectory
            ? throw new EntityNotFoundException()
            : await FileManager.GetFileBytesAsync(container.Id, id);
    }

    [Authorize(FileManagementPermissions.Files.Upload)]
    public virtual async Task<FileDto> CreateAsync(Guid containerId, FileCreateDto input)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var fileBytes = await input.File.GetStream().GetAllBytesAsync();

        var entity = await CreateFileAsync(container, fileName: input.File.FileName!, parentId: input.ParentId ?? Guid.Empty, mimeType: null, fileBytes: fileBytes, overExists: input.Override, extensibleObject: input);

        return ObjectMapper.Map<FileItem, FileDto>(entity);
    }

    [Authorize(FileManagementPermissions.Files.Upload)]
    public virtual async Task<FileDto> CreateByStreamAsync(Guid containerId, FileCreateByStreamDto input)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var bytes = await input.FileStream.GetAllBytesAsync();

        var entity = await CreateFileAsync(container, fileName: input.FileName, parentId: input.ParentId ?? Guid.Empty, mimeType: input.MimeType, fileBytes: bytes, overExists: input.Override, extensibleObject: input);

        return ObjectMapper.Map<FileItem, FileDto>(entity);
    }

    [Authorize(FileManagementPermissions.Files.Upload)]
    public virtual async Task<FileDto> CreateByBytesAsync(Guid containerId, FileCreateByBytesDto input)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var bytes = input.FileData;

        var entity = await CreateFileAsync(container, fileName: input.FileName, parentId: input.ParentId ?? Guid.Empty, mimeType: input.MimeType, fileBytes: bytes, overExists: input.Override, extensibleObject: input);

        return ObjectMapper.Map<FileItem, FileDto>(entity);
    }

    [Authorize(FileManagementPermissions.Files.Update)]
    public virtual async Task<FileDto> MoveAsync(Guid containerId, Guid id, FileMoveAdminRequestDto input)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var entity = await FileRepository.GetAsync(id);

        if (input.TargetId.HasValue && entity.ParentId != input.TargetId)
        {
            var target = await FileManager.FindAsync(input.TargetId.Value);

            if (target?.IsDirectory != false)
            {
                throw new EntityNotFoundException(typeof(FileItem), input.TargetId);
            }

            entity.ChangeParentId(input.TargetId ?? Guid.Empty);
        }

        if (input.TargetContainerId.HasValue && entity.ContainerId != input.TargetContainerId)
        {
            await FileContainerRepository.GetAsync(input.TargetContainerId.Value);

            entity.ChangeContainerId(input.TargetContainerId ?? Guid.Empty);
        }

        if (!input.Override && await FileManager.IsFileExistsAsync(container.Id, entity.FileName, entity.ParentId))
        {
            throw new BusinessException(FileManagementErrorCodes.FileExists);
        }

        await FileRepository.UpdateAsync(entity);

        return ObjectMapper.Map<FileItem, FileDto>(entity);
    }

    [Authorize(FileManagementPermissions.Files.Update)]
    public virtual async Task<FileDto> UpdateAsync(Guid containerId, Guid id, FileUpdateDto input)
    {
        var container = await FileContainerRepository.GetAsync(containerId);

        var entity = await FileRepository.GetAsync(id);

        if (entity.FileName != input.FileName)
        {
            if (await FileManager.IsExistsAsync(container.Id, input.FileName, parentId: entity.ParentId))
            {
                throw new BusinessException(FileManagementErrorCodes.FileExists).WithData("fileName", input.FileName);
            }

            entity.SetFileName(input.FileName);

            input.MapExtraPropertiesTo(entity);

            await FileRepository.UpdateAsync(entity);
        }

        return ObjectMapper.Map<FileItem, FileDto>(entity);
    }

    public virtual async Task<FileDto> CreateDirectoryAsync(Guid containerId, FileDirectoryCreateDto input)
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
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        input.MapExtraPropertiesTo(entity);

        entity = await FileRepository.UpdateAsync(entity!);

        return ObjectMapper.Map<FileItem, FileDto>(entity);
    }

    protected virtual async Task<FileItem> CreateFileAsync(FileContainer container, string fileName, Guid parentId, string? mimeType, byte[] fileBytes, bool overExists = false, ExtensibleObject? extensibleObject = null)
    {
        mimeType ??= FileMimeTypeProvider.Get(fileName);

        var entity = await FileManager.SaveAsync(containerId: container.Id, fileName: fileName, bytes: fileBytes, mimeType: mimeType, parentId: parentId, overrideExisting: overExists);

        await CurrentUnitOfWork.SaveChangesAsync();

        extensibleObject.MapExtraPropertiesTo(entity);

        await FileRepository.UpdateAsync(entity);

        return entity;
    }
}
