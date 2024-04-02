using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public interface IAccountImpersonationAppService : IApplicationService
{
    /// <summary>
    ///  Logout current user and return to impersonator user
    /// </summary>
    Task LogoutAsync();

    /// <summary>
    ///  Login to specified user with id
    /// </summary>
    /// <param name="userId"></param>
    Task LoginAsync(Guid userId);

    /// <summary>
    ///  Link login to specified user with id
    /// </summary>
    /// <param name="userId"></param>
    Task LinkLoginAsync(Guid userId);

    /// <summary>
    ///  Delegation login
    /// </summary>
    /// <param name="id"></param>
    Task DelegationLoginAsync(Guid id);
}
