namespace Passingwind.Abp.FileManagement;

public class FileCompressToBlobRequestDto : FileCompressRequestDto
{
    public bool SaveToFile { get; set; }
}
