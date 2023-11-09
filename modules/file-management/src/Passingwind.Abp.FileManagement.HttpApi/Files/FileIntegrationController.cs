//using System;
//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Volo.Abp;

//namespace Passingwind.Abp.FileManagement.Files;

//[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
//[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
//[ControllerName("UserIntegration")]
//[Route("integration-api/file-management/files")]
//public class FileIntegrationController : FileManagementController, IFileIntegrationAppService
//{
//    private readonly IFileIntegrationAppService _fileIntegrationAppService;

//    public FileIntegrationController(IFileIntegrationAppService fileIntegrationAppService)
//    {
//        _fileIntegrationAppService = fileIntegrationAppService;
//    }

//    public virtual Task<FileDto> CreateByBytesAsync(string containerName, FileCreateByBytesDto input)
//    {
//        return _fileIntegrationAppService.CreateByBytesAsync(containerName, input);
//    }

//    public virtual Task<FileDto> CreateByStreamAsync(string containerName, FileCreateByStreamDto input)
//    {
//        return _fileIntegrationAppService.CreateByStreamAsync(containerName, input);
//    }

//    public virtual Task<FileDto> GetAsync(string containerName, Guid id)
//    {
//        return _fileIntegrationAppService.GetAsync(containerName, id);
//    }

//    public virtual Task<byte[]> GetBytesAsync(string containerName, Guid id)
//    {
//        return _fileIntegrationAppService.GetBytesAsync(containerName, id);
//    }

//    public virtual Task<FileContainerDto> GetContainerAsync(Guid id)
//    {
//        return _fileIntegrationAppService.GetContainerAsync(id);
//    }

//    public virtual Task<Stream?> GetStreamAsync(string containerName, Guid id)
//    {
//        return _fileIntegrationAppService.GetStreamAsync(containerName, id);
//    }
//}
