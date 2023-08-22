using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public class IdentityClientDto : ExtensibleAuditedEntityDto<Guid>
{
    public virtual string Name { get; set; } = null!;
    public virtual string DisplayName { get; set; } = null!;
    public virtual IdentityProviderType ProviderType { get; set; }
    public virtual string ProviderName { get; set; } = null!;
    public virtual bool IsEnabled { get; set; }
    public virtual int DisplayOrder { get; set; }
    public virtual bool IsDebugMode { get; set; }
    public virtual List<IdentityClientClaimMapDto>? ClaimMaps { get; set; }
    public virtual List<string>? RequiredClaimTypes { get; set; }

    public virtual IdentityClientOpenIdConnectConfigurationDto? OpenIdConnectConfiguration { get; set; }
    public virtual IdentityClientSaml2ConfigurationDto? Saml2Configuration { get; set; }
}
