using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileMimeTypeProvider : ITransientDependency
{
    string Get(string fileName);
}
