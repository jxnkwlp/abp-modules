using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public interface IAccountImpersonationAppService : IApplicationService
{
    Task LoginAsync(Guid userId);
    Task LoginLoginAsync(Guid userId);
}
