using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace Passingwind.Abp.Identity;

public class IdentityUserTwoFactorManager : DomainService, IUnitOfWorkEnabled
{
    private readonly IIdentityUserTwoFactorRepository _identityUserTwoFactorRepository;

    public IdentityUserTwoFactorManager(IIdentityUserTwoFactorRepository identityUserTwoFactorRepository)
    {
        _identityUserTwoFactorRepository = identityUserTwoFactorRepository;
    }

    public virtual async Task SetPreferredProviderAsync(IdentityUser user, string providerName, CancellationToken cancellationToken = default)
    {
        var entity = await EnsureEntityAsync(user, cancellationToken);

        entity.PreferredProvider = providerName;

        await _identityUserTwoFactorRepository.UpdateAsync(entity, true, cancellationToken);
    }

    public virtual async Task<string?> GetPreferredProviderAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        var entity = await EnsureEntityAsync(user, cancellationToken);

        return entity.PreferredProvider;
    }

    public virtual async Task SetEmailTokenEnabledAsync(IdentityUser user, bool enabled, CancellationToken cancellationToken = default)
    {
        var entity = await EnsureEntityAsync(user, cancellationToken);

        entity.EmailToken = enabled;

        await _identityUserTwoFactorRepository.UpdateAsync(entity, true, cancellationToken);
    }

    public virtual async Task SetPhoneNumberTokenEnabledAsync(IdentityUser user, bool enabled, CancellationToken cancellationToken = default)
    {
        var entity = await EnsureEntityAsync(user, cancellationToken);

        entity.PhoneNumberToken = enabled;

        await _identityUserTwoFactorRepository.UpdateAsync(entity, true, cancellationToken);
    }

    public virtual async Task<bool> GetEmailTokenEnabledAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        var entity = await EnsureEntityAsync(user, cancellationToken);

        return entity.EmailToken;
    }

    public virtual async Task<bool> GetPhoneNumberTokenEnabledAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        var entity = await EnsureEntityAsync(user, cancellationToken);

        return entity.PhoneNumberToken;
    }

    protected virtual async Task<IdentityUserTwoFactor> EnsureEntityAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        var entity = await _identityUserTwoFactorRepository.FindAsync(user.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            entity = new IdentityUserTwoFactor(user.Id, tenantId: user.TenantId);

            await _identityUserTwoFactorRepository.InsertAsync(entity, true, cancellationToken);
        }

        return entity;
    }
}
