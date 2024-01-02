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

[AllowAnonymous]
public class FileAppService : FileManagementAppService, IFileAppService
{
    protected FileManagementOptions FileManagementOptions { get; }
    protected IFileManager FileManager { get; }
    protected IFileRepository FileRepository { get; }
    protected IFileContainerRepository FileContainerRepository { get; }
    protected IFileContainerManager FileContainerManager { get; }
    protected IFileMimeTypeProvider FileMimeTypeProvider { get; }
    protected IFileRenameProvider FileRenameProvider { get; }

    public FileAppService(
        IOptions<FileManagementOptions> fileManagementOptions,
        IFileManager fileManager,
        IFileRepository fileRepository,
        IFileContainerRepository fileContainerRepository,
        IFileContainerManager fileContainerManager,
        IFileMimeTypeProvider fileMimeTypeProvider,
        IFileRenameProvider fileRenameProvider)
    {
        FileManagementOptions = fileManagementOptions.Value;
        FileManager = fileManager;
        FileRepository = fileRepository;
        FileContainerRepository = fileContainerRepository;
        FileContainerManager = fileContainerManager;
        FileMimeTypeProvider = fileMimeTypeProvider;
        FileRenameProvider = fileRenameProvider;
    }

    public virtual async Task<PagedResultDto<FileDto>> GetListAsync(string containerName, FilePagedListRequestDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var count = await FileRepository.GetCountAsync(
            filter: input.Filter,
            containerId: container.Id,
            parentId: input.ParentId ?? Guid.Empty,
            isDirectory: input.IsDirectory);

        var list = await FileRepository.GetPagedListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Filter,
            containerId: container.Id,
            parentId: input.ParentId ?? Guid.Empty,
            isDirectory: input.IsDirectory,
            sorting: input.Sorting ?? $"{nameof(FileItem.IsDirectory)} desc, {nameof(FileItem.FileName)}");

        return new PagedResultDto<FileDto>()
        {
            Items = ObjectMapper.Map<List<FileItem>, List<FileDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<FileDto> GetAsync(string containerName, Guid id)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        return ObjectMapper.Map<FileItem, FileDto>(entity);
    }

    public virtual async Task<IRemoteStreamContent?> GetBlobAsync(string containerName, Guid id)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        var fileStream = await FileManager.GetFileSteamAsync(container.Id, id);

        return fileStream == null
            ? throw new BlobNotFoundException()
            : new RemoteStreamContent(fileStream, entity.FileName, entity.MimeType);
    }

    public virtual async Task<Stream?> GeBlobStreamAsync(string containerName, Guid id)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        return await FileManager.GetFileSteamAsync(container.Id, id);
    }

    public virtual async Task<byte[]?> GetBlobBytesAsync(string containerName, Guid id)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        return await FileManager.GetFileBytesAsync(container.Id, id);
    }

    public virtual async Task<FileDto> CreateAsync(string containerName, FileCreateDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container, write: true);

        var fileBytes = await input.File.GetStream().GetAllBytesAsync();

        var entity = await CreateFileAsync(container, fileName: input.File.FileName!, parentId: input.ParentId ?? Guid.Empty, mimeType: null, fileBytes: fileBytes, overExists: input.Override, extensibleObject: input);

        return ObjectMapper.Map<FileItem, FileDto>(entity);
    }

    public virtual async Task<FileDto> CreateByStreamAsync(string containerName, FileCreateByStreamDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container, write: true);

        var bytes = await input.FileStream.GetAllBytesAsync();

        var entity = await CreateFileAsync(container, fileName: input.FileName!, parentId: input.ParentId ?? Guid.Empty, mimeType: null, fileBytes: bytes, overExists: input.Override, extensibleObject: input);

        return ObjectMapper.Map<FileItem, FileDto>(entity);
    }

    public virtual async Task<FileDto> CreateByBytesAsync(string containerName, FileCreateByBytesDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container, write: true);

        var bytes = input.FileData;

        var entity = await CreateFileAsync(container, fileName: input.FileName!, parentId: input.ParentId ?? Guid.Empty, mimeType: null, fileBytes: bytes, overExists: input.Override, extensibleObject: input);

        return ObjectMapper.Map<FileItem, FileDto>(entity);
    }

    public virtual async Task<FileDto> MoveAsync(string containerName, Guid id, FileMoveRequestDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container, write: true);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        if (input.TargetId.HasValue && entity.ParentId != input.TargetId)
        {
            var target = await FileManager.FindAsync(input.TargetId.Value);

            if (target?.IsDirectory != false)
            {
                throw new EntityNotFoundException(typeof(FileItem), input.TargetId);
            }

            entity.ChangeParentId(input.TargetId ?? Guid.Empty);
        }

        if (!input.Override && await FileManager.IsFileExistsAsync(container.Id, entity.FileName, entity.ParentId))
        {
            throw new BusinessException(FileManagementErrorCodes.FileExists);
        }

        await FileRepository.UpdateAsync(entity);

        return ObjectMapper.Map<FileItem, FileDto>(entity);
    }

    public virtual async Task<FileDto> RenameAsync(string containerName, Guid id, FileUpdateDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

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

    public virtual async Task<FileDto> CreateDirectoryAsync(string containerName, FileDirectoryCreateDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        var exists = await FileManager.IsDirectoryExistsAsync(container.Id, input.FileName, parentId: input.ParentId);
        if (!input.Force && exists)
        {
            throw new BusinessException(FileManagementErrorCodes.FileExists).WithData("fileName", input.FileName);
        }

        FileItem entity;
        if (exists)
        {
            entity = await FileRepository.GetByNameAsync(container.Id, input.FileName, parentId: input.ParentId, isDirectory: true);
        }
        else
        {
            entity = await FileManager.CreateDirectoryAsync(container.Id, input.FileName, parentId: input.ParentId);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        input.MapExtraPropertiesTo(entity);

        entity = await FileRepository.UpdateAsync(entity);

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

    /// <summary>
    ///  Check current identity can access this container
    /// </summary>
    /// <param name="container"></param>
    /// <param name="write"></param>
    protected override async Task<bool> CanAccessContainerAsync(FileContainer container, bool write = false)
    {
        var isGranted = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainers.Default);

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

    protected virtual async Task CheckContainerPermissionAsync(string containerName, string? policyName = null, bool write = false)
    {
        await CheckContainerPermissionAsync(FileContainerRepository, containerName, policyName, write);
    }
}
