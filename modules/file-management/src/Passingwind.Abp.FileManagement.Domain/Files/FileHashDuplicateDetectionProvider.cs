namespace Passingwind.Abp.FileManagement.Files;

//public class FileHashDuplicateDetectionProvider : IFileDuplicateDetectionProvider
//{
//    private readonly IFileRepository _fileRepository;

//    public FileHashDuplicateDetectionProvider(IFileRepository fileRepository)
//    {
//        _fileRepository = fileRepository;
//    }

//    public async Task<bool> IsExistsAsync(FileContainer fileContainer, File file, CancellationToken cancellationToken = default)
//    {
//        return await _fileRepository.AnyAsync(x => x.Id != file.Id
//                         && x.Hash == file.Hash
//                         && x.ParentId == file.ParentId
//                         && x.ContainerId == file.ContainerId
//                         && x.IsDirectory == file.IsDirectory,
//                         cancellationToken);
//    }
//}