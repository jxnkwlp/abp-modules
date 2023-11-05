using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.Account;

public interface IAccountAuthenticationLoginAppService : IApplicationService
{
    Task<ListResultDto<AccountAuthenticationLoginResultDto>> GetListAsync();
}

public class AccountClaimResultDto
{
    public string ClaimType { get; set; } = null!;
    public string? ClaimValue { get; set; }
}
