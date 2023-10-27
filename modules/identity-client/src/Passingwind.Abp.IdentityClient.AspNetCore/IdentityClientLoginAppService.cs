using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.Application.Services;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Passingwind.Abp.IdentityClient;

public class IdentityClientLoginAppService : ApplicationService, IIdentityClientLoginAppService
{
    protected HttpContext? HttpContext { get; }
    protected IIdentityClientRepository IdentityClientRepository { get; }
    protected SignInManager<IdentityUser> SignInManager { get; }

    public IdentityClientLoginAppService(
        IIdentityClientRepository identityClientRepository,
        SignInManager<IdentityUser> signInManager,
        IHttpContextAccessor httpContextAccessor)
    {
        IdentityClientRepository = identityClientRepository;
        SignInManager = signInManager;
        HttpContext = httpContextAccessor.HttpContext;
    }

    public async Task LoginAsync(string name, string? redirectUrl = null)
    {
        var identityClient = await IdentityClientRepository.FindByProviderNameAsync(name);

        if (identityClient == null || identityClient?.IsEnabled != true)
            throw new BusinessException(IdentityClientErrorCodes.IdentityClientDisabled);

        var properties = SignInManager.ConfigureExternalAuthenticationProperties(identityClient.ProviderName, redirectUrl ?? "/");
        properties.Items["scheme"] = identityClient.ProviderName;

        if (HttpContext == null)
            throw new ArgumentException($"{nameof(HttpContext)} is null");

        await HttpContext.ChallengeAsync(identityClient.ProviderName, properties);
    }
}
