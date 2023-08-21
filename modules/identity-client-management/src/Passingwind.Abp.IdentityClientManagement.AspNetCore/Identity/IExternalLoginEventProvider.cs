using System.Threading.Tasks;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public interface IExternalLoginEventProvider
{
    Task RedirectToIdentityProviderAsync(ExternalLoginRedirectToIdentityProviderContext context);

    Task MessageReceivedAsync(ExternalLoginMessageReceivedContext context);

    Task LoginInfoReceivedAsync(ExternalLoginInfoReceivedContext context);

    Task SignInAsync(ExternalLoginSignInContext context);

    Task UserSignInAsync(ExternalLoginUserSignInContext context);
}
