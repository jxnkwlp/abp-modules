using System.Threading.Tasks;

namespace Passingwind.Abp.Account;

public interface IAccountExternalProvider
{
    Task LoginInfoReceivedAsync(AccountExternalCallbackLoginInfoContext context);

    Task SignInAsync(AccountExternalCallbackSignInContext context);

    Task UserSignInAsync(AccountExternalCallbackUserSignInContext context);
}
