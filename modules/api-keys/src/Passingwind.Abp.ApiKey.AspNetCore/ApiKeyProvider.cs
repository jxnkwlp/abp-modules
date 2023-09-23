using System;
using System.Threading;
using System.Threading.Tasks;
using Passingwind.Abp.Identity;
using Passingwind.AspNetCore.Authentication.ApiKey;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Passingwind.Abp.ApiKey;

public class ApiKeyProvider : IApiKeyProvider
{
    protected IClock Clock { get; }
    protected IApiKeyRecordManager ApiKeyRecordManager { get; }
    protected IAbpClaimsPrincipalFactory PrincipalFactory { get; }
    protected IdentityUserManager UserManager { get; }
    protected AbpSignInManager SignInManager { get; }

    public ApiKeyProvider(
        IClock clock,
        IApiKeyRecordManager apiKeyRecordManager,
        IAbpClaimsPrincipalFactory principalFactory,
        IdentityUserManager userManager,
        AbpSignInManager signInManager)
    {
        Clock = clock;
        ApiKeyRecordManager = apiKeyRecordManager;
        PrincipalFactory = principalFactory;
        UserManager = userManager;
        SignInManager = signInManager;
    }

    [UnitOfWork]
    public virtual async Task<ApiKeyValidationResult> ValidateAsync(string apiKey, CancellationToken cancellationToken = default)
    {
        var cacheItem = await ApiKeyRecordManager.FindFromCacheAsync(apiKey, cancellationToken);

        if (cacheItem?.CacheId.HasValue != true)
            return ApiKeyValidationResult.Failed(new Exception("Invalid api key"));

        var record = await ApiKeyRecordManager.FindByIdAsync(cacheItem.CacheId!.Value, cancellationToken);

        if (record.ExpirationTime.HasValue && Clock.Now > record.ExpirationTime)
            return ApiKeyValidationResult.Failed(new Exception("Api key has expired"));

        var user = await UserManager.FindByIdAsync(record.UserId.ToString());

        if (user == null)
            return ApiKeyValidationResult.Failed(new Exception("Invalid api key"));

        var principal = await SignInManager.ClaimsFactory.CreateAsync(user);

        principal = await PrincipalFactory.CreateAsync(principal);

        return ApiKeyValidationResult.Success(new System.Security.Claims.ClaimsIdentity(principal.Identity));
    }
}
