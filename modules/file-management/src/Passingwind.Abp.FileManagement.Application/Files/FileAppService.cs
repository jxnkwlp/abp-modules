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
using Volo.Abp.Authorization;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement.Files;

[Authorize]
public class FileAppService : FileManagementAppService, IFileAppService
{
    private readonly IFileManager _fileManager;
    private readonly IFileRepository _fileRepository;
    private readonly IFileContainerRepository _fileContainerRepository;
    private readonly FileContainerManager _fileContainerManager;
    private readonly IFileInfoCheckProvider _fileInfoCheckProvider;
    private readonly IFileMimeTypeProvider _fileMimeTypeProvider;
    private readonly IFileRenameProvider _fileRenameProvider;
    private readonly IFileAccessTokenProvider _fileAccessTokenProvider;
    private readonly FileManagementOptions _options;

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
        _fileManager = fileManager;
        _fileRepository = fileRepository;
        _fileContainerRepository = fileContainerRepository;
        _fileContainerManager = fileContainerManager;
        _fileInfoCheckProvider = fileInfoCheckProvider;
        _fileMimeTypeProvider = fileMimeTypeProvider;
        _fileRenameProvider = fileRenameProvider;
        _fileAccessTokenProvider = fileAccessTokenProvider;
        _options = options.Value;
    }

    public virtual async Task<PagedResultDto<FileDto>> GetListAsync(string containerName, FileListRequestDto input)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var count = await _fileRepository.GetCountAsync(filter: input.Filter, containerId: container.Id, parentId: input.ParentId);
        var list = await _fileRepository.GetPagedListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Filter,
            container.Id,
            input.ParentId,
            nameof(File.FileName));

        return new PagedResultDto<FileDto>()
        {
            Items = ObjectMapper.Map<List<File>, List<FileDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<FileDto> GetAsync(string containerName, Guid id)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await _fileRepository.GetAsync(id);

        await CheckContainerHasFileAsync(container, entity);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public virtual async Task DeleteAsync(string containerName, Guid id)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        // 1: container permssion
        await CheckContainerPermissionAsync(container);

        //2: file permssion 
        await AuthorizationService.CheckAsync(FileManagementPermissions.File.Delete);

        var entity = await _fileRepository.GetAsync(id);

        await CheckContainerHasFileAsync(container, entity);

        await _fileRepository.DeleteAsync(id);
    }

    public async Task<IRemoteStreamContent?> GetBlobAsync(string containerName, Guid id)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckFileAccessAsync(container);

        var entity = await _fileRepository.GetAsync(id);

        await CheckContainerHasFileAsync(container, entity);

        var fileStream = await _fileManager.GetFileSteamAsync(container, entity);

        if (fileStream == null)
            throw new BlobNotFoundException();

        return new RemoteStreamContent(fileStream, entity.FileName, entity.MimeType);
    }

    public async Task<System.IO.Stream?> GeBlobStreamAsync(string containerName, Guid id)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckFileAccessAsync(container);

        var entity = await _fileRepository.GetAsync(id);

        await CheckContainerHasFileAsync(container, entity);

        return await _fileManager.GetFileSteamAsync(container, entity);
    }

    public async Task<byte[]> GetBlobBytesAsync(string containerName, Guid id)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckFileAccessAsync(container);

        var entity = await _fileRepository.GetAsync(id);

        await CheckContainerHasFileAsync(container, entity);

        return await _fileManager.GetFileBytesAsync(container, entity);
    }

    public async Task<FileDto> CreateAsync(string containerName, FileCreateDto input)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var fileBytes = await input.File.GetStream().GetAllBytesAsync();

        var entity = await CreateFileAsync(container, input.File.FileName, null, fileBytes, input);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public async Task<FileDto> CreateByStreamAsync(string containerName, FileCreateByStreamDto input)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var bytes = await input.FileStream.GetAllBytesAsync();

        var entity = await CreateFileAsync(container, input.FileName, input.MimeType, bytes, input);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public async Task<FileDto> CreateByBytesAsync(string containerName, FileCreateByBytesDto input)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var bytes = input.FileData;

        var entity = await CreateFileAsync(container, input.FileName, input.MimeType, bytes, input);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public async Task<FileDto> MoveAsync(string containerName, Guid id, FileMoveRequestDto input)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await _fileRepository.GetAsync(id);

        await CheckContainerHasFileAsync(container, entity);

        entity = await _fileManager.ChangeFileNameAsync(container, entity, entity.FileName, input.TargetId);

        await _fileInfoCheckProvider.CheckAsync(container, entity);

        await _fileRepository.UpdateAsync(entity);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public async Task<FileDto> UpdateAsync(string containerName, Guid id, FileUpdateDto input)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await _fileRepository.GetAsync(id);

        await CheckContainerHasFileAsync(container, entity);

        entity = await _fileManager.ChangeFileNameAsync(container, entity, input.FileName, entity.ParentId);

        await _fileInfoCheckProvider.CheckAsync(container, entity);

        await _fileRepository.UpdateAsync(entity);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public async Task<FileDto> CreateDirectoryAsync(string containerName, FileDirectoryCreateDto input)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await _fileManager.CreateDirectoryAsync(container, input.FileName, input.ParentId);

        if (await _fileManager.IsDirectoryExistsAsync(container, entity))
        {
            throw new BusinessException(FileManagementErrorCodes.DirectoryExists);
        }

        await _fileRepository.InsertAsync(entity);

        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public async Task<FileDownloadInfoResultDto> CreateDownloadInfoAsync(string containerName, Guid id, FileDownloadInfoRequestDto input)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        await CheckContainerPermissionAsync(container);

        var entity = await _fileRepository.GetAsync(id);

        await CheckContainerHasFileAsync(container, entity);

        TimeSpan? expiration = input.ExpirationSecond.HasValue ? TimeSpan.FromSeconds(input.ExpirationSecond.Value) : null;
        var token = await _fileAccessTokenProvider.CreateAsync(container, entity, expiration);

        return new FileDownloadInfoResultDto
        {
            FileName = entity.FileName,
            Length = entity.Length,
            ExpirationTime = expiration.HasValue ? Clock.Now.Add(expiration.Value) : null,
            DownloadUrl = string.Format(_options.FileDownloadUrlFormat, token),
            Token = token,
        };
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent?> DownloadAsync(string token)
    {
        var file = await _fileAccessTokenProvider.ValidAsync(token);

        if (file == null)
            throw new BusinessException(FileManagementErrorCodes.DownloadNotExistsOrExpired);

        var container = await _fileContainerRepository.FindAsync(file.ContainerId);

        if (container == null)
            throw new BusinessException(FileManagementErrorCodes.DownloadNotExistsOrExpired);

        var fileStream = await _fileManager.GetFileSteamAsync(container, file);

        if (fileStream == null)
            throw new BlobNotFoundException();

        return new RemoteStreamContent(fileStream, file.FileName, file.MimeType);
    }

    protected async Task<File> CreateFileAsync(FileContainer container, string fileName, string? mimeType, byte[] fileBytes, ExtensibleObject extensibleObject)
    {
        mimeType ??= _fileMimeTypeProvider.Get(fileName);

        var entity = await _fileManager.CreateFileAsync(container, fileName, mimeType, fileBytes);

        extensibleObject.MapExtraPropertiesTo(entity);

        await _fileInfoCheckProvider.CheckAsync(container, entity);

        var isExists = await _fileManager.IsFileExistsAsync(container, entity);

        if (isExists && container.OverrideBehavior == FileOverrideBehavior.Override)
        {
            entity = await _fileManager.FindFileAsync(container, entity.FileName, entity.ParentId);

            if (entity == null)
                throw new EntityNotFoundException();

            // update
            entity = await _fileManager.UpdateFileAsync(container, entity, fileBytes);

            await _fileRepository.UpdateAsync(entity);
        }
        else if (isExists && container.OverrideBehavior == FileOverrideBehavior.Rename)
        {
            var newFileName = await _fileRenameProvider.GetAsync(container, entity.FileName, entity.ParentId);

            entity.SetFileName(newFileName);

            // new 
            await _fileRepository.InsertAsync(entity);
        }
        else
        {
            // new 
            await _fileRepository.InsertAsync(entity);
        }

        // save to blob
        await _fileManager.SaveBlobAsync(container, entity, fileBytes);

        return entity;
    }

    protected virtual async Task CheckFileAccessAsync(string containerName)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);
        await CheckFileAccessAsync(container);
    }

    protected virtual async Task CheckFileAccessAsync(FileContainer container, string? policyName = null)
    {
        await CheckContainerPermissionAsync(container);
    }

    /// <summary>
    ///  Check current identity can access this container
    /// </summary> 
    protected virtual async Task<bool> CanAccessContainerAsync(FileContainer container)
    {
        bool isGranted = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileContainer.Default);

        if (isGranted)
            return true;

        if (container.AccessMode == FileAccessMode.Public)
            return true;
        else if (container.AccessMode == FileAccessMode.Authorize && CurrentUser.IsAuthenticated)
            return true;
        else
            // private & owner
            return container.CreatorId == CurrentUser.Id;
    }

    protected virtual async Task CheckContainerPermissionAsync(string containerName, string? policyName = null)
    {
        var container = await _fileContainerRepository.GetByNameAsync(containerName);

        if (!await CanAccessContainerAsync(container))
        {
            throw new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenPolicyHasNotGrantedWithPolicyName)
                 .WithData("PolicyName", policyName ?? FileManagementPermissions.FileContainer.Default);
        }
    }

    protected virtual async Task CheckContainerPermissionAsync(FileContainer container, string? policyName = null)
    {
        if (!await CanAccessContainerAsync(container))
        {
            throw new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenPolicyHasNotGrantedWithPolicyName)
                 .WithData("PolicyName", policyName ?? FileManagementPermissions.FileContainer.Default);
        }
    }

    protected virtual Task CheckContainerHasFileAsync(FileContainer container, File file)
    {
        if (container.Id != file.ContainerId)
            throw new EntityNotFoundException();

        return Task.CompletedTask;
    }
}
