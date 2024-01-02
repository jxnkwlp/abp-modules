using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.FileManagement;

public interface IFileMimeTypeProvider : ITransientDependency
{
    string Get(string fileName);
}
