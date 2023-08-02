using System;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileAccessTokenRepository : IRepository<FileAccessToken, Guid>
{
}
