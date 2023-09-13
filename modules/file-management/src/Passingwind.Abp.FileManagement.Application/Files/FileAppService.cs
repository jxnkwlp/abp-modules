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

namespace Passingwind.Abp.FileManagement.Files;

[AllowAnonymous]
public class FileAppService : FileManagementAppService, IFileAppService
{
    protected IFileManager FileManager;
    protected IFileRepository FileRepository;
    protected IFileContainerRepository FileContainerRepository;
    protected FileContainerManager FileContainerManager;
    protected IFileInfoCheckProvider FileInfoCheckProvider;
    protected IFileMimeTypeProvider FileMimeTypeProvider;
    protected IFileRenameProvider FileRenameProvider;
    protected IFileAccessTokenProvider FileAccessTokenProvider;
    protected FileManagementOptions FileManagementOptions;

    public FileAppService(
        IFileManager fileManager,
        IFileRepository fileRepository,
        IFileContainerRepository fileContainerRepository,
        FileContainerManager fileContainerManager,
        IFileInfoCheckProvider fileInfoCheckProvider,
        IFileMimeTypeProvider fileMimeTypeProvider,
        IFileRenameProvider fileRenameProvider,
        IFileAccessTokenProvider fileAccessTokenProvider,
        IOptions<FileManagementOptions> options)
    {
        FileManager = fileManager;
        FileRepository = fileRepository;
        FileContainerRepository = fileContainerRepository;
        FileContainerManager = fileContainerManager;
        FileInfoCheckProvider = fileInfoCheckProvider;
        FileMimeTypeProvider = fileMimeTypeProvider;
        FileRenameProvider = fileRenameProvider;
        FileAccessTokenProvider = fileAccessTokenProvider;
        FileManagementOptions = options.Value;
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
            sorting: input.Sorting ?? $"{nameof(File.IsDirectory)} desc,{nameof(File.FileName)}");

        return new PagedResultDto<FileDto>()
        {
            Items = ObjectMapper.Map<List<File>, List<FileDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<FileDto> GetAsync(string containerName, Guid id)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public virtual async Task DeleteAsync(string containerName, Guid id)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        // 1: container permssion
        await CheckContainerPermissionAsync(container, write: true);

        //2: file permssion 
        await AuthorizationService.CheckAsync(FileManagementPermissions.File.Delete);

        var entity = await FileRepository.GetAsync(id);

        if (entity.IsDirectory)
        {
            // if an directory has files, can't be delete
            var fileCount = await FileRepository.GetCountAsync(containerId: container.Id, parentId: entity.Id);
            if (fileCount > 0)
            {
                throw new BusinessException(FileManagementErrorCodes.DirectoryHasFiles).WithData("name", entity.FileName);
            }

            await FileRepository.DeleteAsync(entity);
        }
        else
        {
            await CheckFileIsInContainerAsync(container, entity);

            await FileManager.DeleteAsync(container, entity);
        }
    }

    public virtual async Task<IRemoteStreamContent?> GetBlobAsync(string containerName, Guid id)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        var fileStream = await FileManager.GetFileSteamAsync(container, entity);

        if (fileStream == null)
            throw new BlobNotFoundException();

        return new RemoteStreamContent(fileStream, entity.FileName, entity.MimeType);
    }

    public virtual async Task<System.IO.Stream?> GeBlobStreamAsync(string containerName, Guid id)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        return await FileManager.GetFileSteamAsync(container, entity);
    }

    public virtual async Task<byte[]> GetBlobBytesAsync(string containerName, Guid id)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        return await FileManager.GetFileBytesAsync(container, entity);
    }

    public virtual async Task<FileDto> CreateAsync(string containerName, FileCreateDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container, write: true);

        var fileBytes = await input.File.GetStream().GetAllBytesAsync();

        var entity = await CreateFileAsync(container, input.File.FileName, input.ParentId, null, fileBytes, input);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public virtual async Task<FileDto> CreateByStreamAsync(string containerName, FileCreateByStreamDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container, write: true);

        var bytes = await input.FileStream.GetAllBytesAsync();

        var entity = await CreateFileAsync(container, input.FileName, input.ParentId, input.MimeType, bytes, input);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public virtual async Task<FileDto> CreateByBytesAsync(string containerName, FileCreateByBytesDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container, write: true);

        var bytes = input.FileData;

        var entity = await CreateFileAsync(container, input.FileName, input.ParentId, input.MimeType, bytes, input);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public virtual async Task<FileDto> MoveAsync(string containerName, Guid id, FileMoveRequestDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container, write: true);

        var entity = await FileRepository.GetAsync(id);

        await CheckFileIsInContainerAsync(container, entity);

        entity = await FileManager.ChangeNameAsync(container, entity, entity.FileName, input.TargetId);

        await FileInfoCheckProvider.CheckAsync(container, entity);

        await FileRepository.UpdateAsync(entity);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public virtual async Task<FileDto> UpdateAsync(string containerName, Guid id, FileUpdateDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await FileRepository.GetAsync(id);

        if (!entity.IsDirectory)
            await CheckFileIsInContainerAsync(container, entity);

        entity = await FileManager.ChangeNameAsync(container, entity, input.FileName, entity.ParentId);

        await FileInfoCheckProvider.CheckAsync(container, entity);

        input.MapExtraPropertiesTo(entity);

        await FileRepository.UpdateAsync(entity);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public virtual async Task<FileDto> CreateDirectoryAsync(string containerName, FileDirectoryCreateDto input)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container, write: true);

        var entity = await FileManager.CreateDirectoryAsync(container, input.FileName, input.ParentId);

        input.MapExtraPropertiesTo(entity);

        bool exists = await FileManager.IsDirectoryExistsAsync(container, entity);
        if (!exists)
        {
            await FileRepository.InsertAsync(entity);

            return ObjectMapper.Map<File, FileDto>(entity);
        }

        if (exists && !input.Force)
        {
            throw new BusinessException(FileManagementErrorCodes.DirectoryExists).WithData("name", entity.FileName);
        }

        entity = await FileRepository.GetByNameAsync(container.Id, input.FileName, input.ParentId);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    protected virtual async Task<File> CreateFileAsync(FileContainer container, string fileName, Guid? parentId, string? mimeType, byte[] fileBytes, ExtensibleObject extensibleObject)
    {
        mimeType ??= FileMimeTypeProvider.Get(fileName);

        var entity = await FileManager.CreateFileAsync(container, fileName, mimeType, fileBytes, parentId);

        extensibleObject.MapExtraPropertiesTo(entity);

        await FileInfoCheckProvider.CheckAsync(container, entity);

        var isExists = await FileManager.IsFileExistsAsync(container, entity);

        if (isExists && container.OverrideBehavior == FileOverrideBehavior.Override)
        {
            entity = await FileManager.FindFileAsync(container, entity.FileName, entity.ParentId);

            if (entity == null)
                throw new EntityNotFoundException();

            // update
            entity = await FileManager.UpdateFileAsync(container, entity, fileBytes);

            await FileRepository.UpdateAsync(entity);
        }
        else if (isExists && container.OverrideBehavior == FileOverrideBehavior.Rename)
        {
            var newFileName = await FileRenameProvider.GetAsync(container, entity.FileName, entity.ParentId);

            entity.SetFileName(newFileName);

            // new 
            await FileRepository.InsertAsync(entity);
        }
        else
        {
            // new 
            await FileRepository.InsertAsync(entity);
        }

        // save to blob
        await FileManager.SaveBlobAsync(container, entity, fileBytes);

        return entity;
    }

    /// <summary>
    ///  Check current identity can access this container
    /// </summary>
    /// <param name="container"></param>
    /// <param name="write"></param>
    protected override async Task<bool> CanAccessContainerAsync(FileContainer container, bool write = false)
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

    protected virtual async Task CheckContainerPermissionAsync(string containerName, string? policyName = null, bool write = false)
    {
        await CheckContainerPermissionAsync(FileContainerRepository, containerName, policyName, write);
    }
}
