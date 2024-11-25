using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Identity;

public interface IAccountLinkUserAppService : IApplicationService
{
    /// <summary>
    ///  Get current user account link list
    /// </summary>
    Task<ListResultDto<AccountLinkUserDto>> GetListAsync(AccountLinkUserListRequestDto input);

    /// <summary>
    ///  Generate link token for current account
    /// </summary>
    Task<AccountLinkDto> CreateLinkTokenAsync();

    /// <summary>
    ///  Verify the token for specify user, if true, link to current user
    /// </summary>
    Task<AccountLinkTokenValidationResultDto> VerifyLinkTokenAsync(AccountLinkTokenValidationRequestDto input);

    /// <summary>
    ///  Unlink specify user for current account
    /// </summary>
    Task UnlinkAsync(AccountUnlinkDto input);
}
