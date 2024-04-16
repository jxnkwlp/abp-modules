using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Passingwind.Abp.Account;

[Authorize]
public class AccountUserDelegationAppService : AccountAppBaseService, IAccountUserDelegationAppService
{
    protected IIdentityUserRepository UserRepository { get; }
    protected IdentityUserDelegationManager UserDelegationManager { get; }

    public AccountUserDelegationAppService(IIdentityUserRepository userRepository, IdentityUserDelegationManager userDelegationManager)
    {
        UserRepository = userRepository;
        UserDelegationManager = userDelegationManager;
    }

    /// <inheritdoc/>
    public virtual async Task<ListResultDto<AccountUserDelegationDto>> GetMyDelegatedListAsync()
    {
        var list = await UserDelegationManager.GetListAsync(targetUserId: CurrentUser.GetId());

        var users = await UserRepository.GetListByIdsAsync(list.Select(x => x.SourceUserId).Distinct());

        return new ListResultDto<AccountUserDelegationDto>(list.ConvertAll(x => new AccountUserDelegationDto
        {
            Id = x.Id,
            UserId = x.SourceUserId,
            UserName = users.Find(u => u.Id == x.SourceUserId)?.UserName,
            EndTime = x.EndTime,
            StartTime = x.StartTime,
            IsActive = Clock.Now >= x.StartTime && Clock.Now <= x.EndTime,
        }));
    }

    /// <inheritdoc/>
    public virtual async Task<ListResultDto<AccountUserDelegationDto>> GetDelegatedListAsync()
    {
        var list = await UserDelegationManager.GetListAsync(sourceUserId: CurrentUser.GetId());

        var users = await UserRepository.GetListByIdsAsync(list.Select(x => x.TargetUserId).Distinct());

        return new ListResultDto<AccountUserDelegationDto>(list.ConvertAll(x => new AccountUserDelegationDto
        {
            Id = x.Id,
            UserId = x.TargetUserId,
            UserName = users.Find(u => u.Id == x.TargetUserId)?.UserName,
            EndTime = x.EndTime,
            StartTime = x.StartTime,
            IsActive = Clock.Now >= x.StartTime && Clock.Now <= x.EndTime,
        }));
    }

    /// <inheritdoc/>
    public virtual async Task CreateAsync(AccountUserDelegationCreateDto input)
    {
        if (CurrentUser.FindImpersonatorUserId().HasValue)
        {
            throw new BusinessException(AccountErrorCodes.UserActionDisabledInDelegatedMode);
        }

        if (CurrentUser.Id == input.UserId)
        {
            throw new BusinessException(AccountErrorCodes.UserNotFound);
        }

        var user = await UserRepository.FindAsync(input.UserId);

        if (user == null)
        {
            throw new BusinessException(AccountErrorCodes.UserNotFound);
        }

        await UserDelegationManager.DelegateNewUserAsync(
            sourceUserId: CurrentUser.GetId(),
            targetUserId: input.UserId,
            startTime: input.StartTime,
            endTime: input.EndTime);
    }

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(Guid id)
    {
        if (CurrentUser.FindImpersonatorUserId().HasValue)
        {
            throw new BusinessException(AccountErrorCodes.UserActionDisabledInDelegatedMode);
        }

        await UserDelegationManager.DeleteDelegationAsync(id, CurrentUser.GetId());
    }

    /// <inheritdoc/>
    public virtual async Task<ListResultDto<UserBasicDto>> UserLookupAsync(string? filter = null)
    {
        var list = await UserRepository.GetListAsync(sorting: nameof(IdentityUser.UserName), maxResultCount: 10, filter: filter);

        return new ListResultDto<UserBasicDto>(ObjectMapper.Map<List<IdentityUser>, List<UserBasicDto>>(list));
    }
}
