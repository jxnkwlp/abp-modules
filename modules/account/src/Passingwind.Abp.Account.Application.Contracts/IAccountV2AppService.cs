using System.Threading.Tasks;
using Volo.Abp.Account;

namespace Passingwind.Abp.Account;

public interface IAccountV2AppService : IAccountAppService
{
    Task<AccountVerifyPasswordResetTokenResultDto> VerifyPasswordResetTokenV2Async(VerifyPasswordResetTokenInput input);
}
