using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Content;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace Passingwind.Abp.FileManagement;

[Authorize]
public class FileCompressionAppService : FileManagementAppService, IFileCompressionAppService
{
    private readonly IFileItemRepository _fileRepository;
    private readonly IFileManager _fileManager;
    private readonly FileContainerManager _fileContainerManager;
    private readonly IFileCompressionProvider _fileCompressionProvider;
    private readonly IFileMimeTypeProvider _fileMimeTypeProvider;
    private readonly IFileRenameProvider _fileRenameProvider;

    public FileCompressionAppService(IFileItemRepository fileRepository, IFileManager fileManager, FileContainerManager fileContainerManager, IFileCompressionProvider fileCompressionProvider, IFileMimeTypeProvider fileMimeTypeProvider, IFileRenameProvider fileRenameProvider)
    {
        _fileRepository = fileRepository;
        _fileManager = fileManager;
        _fileContainerManager = fileContainerManager;
        _fileCompressionProvider = fileCompressionProvider;
        _fileMimeTypeProvider = fileMimeTypeProvider;
        _fileRenameProvider = fileRenameProvider;
    }

    public virtual async Task<FileItemDto> CompressToFileAsync(string containerName, FileCompressRequestDto input)
    {
        var container = await _fileContainerManager.GetByNameAsync(containerName);

        var resultStream = await CompressToStreamAsync(containerName, input);

        if (resultStream == null)
            throw new BlobNotFoundException();

        if (!input.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            input.FileName += ".zip";

        resultStream.Seek(0, SeekOrigin.Begin);

        var file = await CreateFileAsync(container, input.FileName, resultStream);

        return ObjectMapper.Map<FileItem, FileItemDto>(file);
    }

    public virtual async Task<Stream?> CompressToStreamAsync(string containerName, FileCompressRequestDto input)
    {
        var container = await _fileContainerManager.GetByNameAsync(containerName);

        if (input.Ids?.Length == 0)
        {
            throw new AbpValidationException(nameof(input.Ids));
        }

        var list = await _fileRepository.GetListByIdsAsync(input.Ids!);

        var files = new List<FileDescriptor>();

        foreach (var item in list)
        {
            var stream = await _fileManager.GetFileSteamAsync(item.Id);
            if (stream == null)
            {
                Logger.LogWarning("The container '{ContainerName}' file '{FileName}' not found", containerName, item.FileName);
                continue;
            }

            files.Add(new FileDescriptor
            {
                FileName = item.FileName,
                Stream = stream,
                Created = item.CreationTime,
            });
        }

        try
        {
            return await _fileCompressionProvider.CompressAsync(files);
        }
        catch (Exception ex)
        {
            throw new BusinessException(FileManagementErrorCodes.FileCompressFailed, innerException: ex);
        }
    }

    public virtual async Task<IRemoteStreamContent> CompressAsync(string containerName, FileCompressToBlobRequestDto input)
    {
        var container = await _fileContainerManager.GetByNameAsync(containerName);

        var resultStream = await CompressToStreamAsync(containerName, input);

        if (resultStream == null)
            throw new BlobNotFoundException();

        if (!input.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            input.FileName += ".zip";

        if (input.SaveToFile)
        {
            await CreateFileAsync(container, input.FileName, resultStream);
        }

        resultStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(resultStream, input.FileName, "application/stream");
    }

    public virtual async Task<FileDecompressResultDto> DecompressAsync(string containerName, Guid id, FileDecompressRequestDto input)
    {
        var container = await _fileContainerManager.GetByNameAsync(containerName);
        var file = await _fileRepository.GetAsync(id);

        var sourceFileStream = await _fileManager.GetFileSteamAsync(file.Id);

        if (sourceFileStream == null)
        {
            throw new BlobNotFoundException();
        }

        string? directoryName = string.Empty;
        if (input.AutoDirectoryName)
        {
            directoryName = await _fileRenameProvider.RenameAsync(container.Name, fileName: Path.GetFileNameWithoutExtension(file.FileName), parentId: file.ParentId);
        }
        else if (!string.IsNullOrWhiteSpace(input.DirectoryName))
        {
            directoryName = await _fileRenameProvider.RenameAsync(container.Name, fileName: input.DirectoryName, parentId: file.ParentId);
        }

        FileItem? fileDirectory = null;
        if (!string.IsNullOrWhiteSpace(directoryName))
        {
            fileDirectory = await _fileManager.CreateDirectoryAsync(container.Id, directoryName, file.ParentId);
            await CurrentUnitOfWork!.SaveChangesAsync();
        }

        // TODO: decompresion in background job
        // input.Background

        ImmutableArray<FileDescriptor> decompressFiles;

        try
        {
            decompressFiles = await _fileCompressionProvider.DecompressAsync(sourceFileStream);
        }
        catch (Exception ex)
        {
            throw new BusinessException(FileManagementErrorCodes.FileDecompressFailed, innerException: ex);
        }

        foreach (var item in decompressFiles)
        {
            await CreateFileAsync(container!, item.FileName, item.Stream, fileDirectory);
        }

        return new FileDecompressResultDto { Background = input.Background };
    }

    [UnitOfWork]
    protected virtual async Task<FileItem> CreateFileAsync(FileContainer fileContainer, string fileName, Stream stream, FileItem? directory = null, CancellationToken cancellationToken = default)
    {
        await using var memoryStram = new MemoryStream();
        await stream.CopyToAsync(memoryStram, cancellationToken);
        var bytes = memoryStram.ToArray();

        var mimeType = _fileMimeTypeProvider.Get(fileName);

        fileName = await _fileRenameProvider.RenameAsync(container: fileContainer, fileName: fileName, parentId: directory?.Id ?? Guid.Empty, cancellationToken: cancellationToken);

        var file = await _fileManager.SaveAsync(containerId: fileContainer.Id, fileName: fileName, bytes: bytes!, mimeType: mimeType, parentId: directory?.Id ?? Guid.Empty, cancellationToken: cancellationToken);

        await CurrentUnitOfWork!.SaveChangesAsync(cancellationToken);

        return file;
    }
}
