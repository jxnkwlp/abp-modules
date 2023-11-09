using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.IdentityClient.Identity;
using Passingwind.Abp.IdentityClient.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.IdentityClient;

[Authorize(IdentityClientPermissions.IdentityClient.Default)]
public class IdentityClientAppService : IdentityClientAppBaseService, IIdentityClientAppService
{
    private readonly IdentityClientManager _identityClientManager;
    private readonly IIdentityClientRepository _identityClientRepository;
    private readonly IIdentityClientRegisterProvider _identityClientRegisterProvider;
    private readonly IIdentityClientProviderNameProvider _identityClientProviderNameProvider;

    public IdentityClientAppService(
        IdentityClientManager identityClientManager,
        IIdentityClientRepository identityClientRepository,
        IIdentityClientRegisterProvider identityClientRegisterProvider,
        IIdentityClientProviderNameProvider identityClientProviderNameProvider)
    {
        _identityClientManager = identityClientManager;
        _identityClientRepository = identityClientRepository;
        _identityClientRegisterProvider = identityClientRegisterProvider;
        _identityClientProviderNameProvider = identityClientProviderNameProvider;
    }

    public virtual async Task<PagedResultDto<IdentityClientDto>> GetListAsync(IdentityClientListRequestDto input)
    {
        var count = await _identityClientRepository.GetCountAsync();
        var list = await _identityClientRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, sorting: nameof(IdentityClient.CreationTime) + " desc");

        return new PagedResultDto<IdentityClientDto>()
        {
            Items = ObjectMapper.Map<List<IdentityClient>, List<IdentityClientDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<IdentityClientDto> GetAsync(Guid id)
    {
        var entity = await _identityClientRepository.GetAsync(id);

        var dto = ObjectMapper.Map<IdentityClient, IdentityClientDto>(entity);

        if (entity.ProviderType == IdentityProviderType.OpenIdConnect)
        {
            var configuration = IdentityClientConfigurationHelper.ToOpenIdConnectConfiguration(entity.Configurations);
            dto.OpenIdConnectConfiguration = ObjectMapper.Map<IdentityClientOpenIdConnectConfiguration, IdentityClientOpenIdConnectConfigurationDto>(configuration);
        }
        else if (entity.ProviderType == IdentityProviderType.Saml2)
        {
            var configuration = IdentityClientConfigurationHelper.ToSaml2Configuration(entity.Configurations);
            dto.Saml2Configuration = ObjectMapper.Map<IdentityClientSaml2Configuration, IdentityClientSaml2ConfigurationDto>(configuration);
        }

        return dto;
    }

    [Authorize(IdentityClientPermissions.IdentityClient.Create)]
    public virtual async Task<IdentityClientDto> CreateAsync(IdentityClientCreateDto input)
    {
        if (await _identityClientRepository.IsNameExistsAsync(input.Name))
        {
            throw new BusinessException(IdentityClientErrorCodes.IdentityClientNameExists).WithData("name", input.Name);
        }

        var entity = new IdentityClient(GuidGenerator.Create(), input.Name, CurrentTenant.Id)
        {
            DisplayName = input.DisplayName,
            ProviderType = input.ProviderType,
            IsEnabled = input.IsEnabled,
            DisplayOrder = input.DisplayOrder,
            IsDebugMode = input.IsDebugMode,
            RequiredClaimTypes = input.RequiredClaimTypes ?? new List<string>(),
            ClaimMaps = input.ClaimMaps?.ConvertAll(x => new IdentityClientClaimMap()
            {
                ClaimType = x.ClaimType,
                Action = x.Action,
                ValueFromType = x.ValueFromType,
                RawValue = x.RawValue,
            }) ?? new List<IdentityClientClaimMap>()
        };

        input.MapExtraPropertiesTo(entity);

        entity.ProviderName = await _identityClientProviderNameProvider.CreateAsync(entity);

        if (input.OpenIdConnectConfiguration != null)
        {
            entity.Configurations = IdentityClientConfigurationHelper.ToConfigurations(ObjectMapper.Map<IdentityClientOpenIdConnectConfigurationDto, IdentityClientOpenIdConnectConfiguration>(input.OpenIdConnectConfiguration)).ToList();
        }
        else if (input.ProviderType == IdentityProviderType.Saml2 && input.Saml2Configuration != null)
        {
            entity.Configurations = IdentityClientConfigurationHelper.ToConfigurations(ObjectMapper.Map<IdentityClientSaml2ConfigurationDto, IdentityClientSaml2Configuration>(input.Saml2Configuration)).ToList();
        }

        await _identityClientRepository.InsertAsync(entity);

        return ObjectMapper.Map<IdentityClient, IdentityClientDto>(entity);
    }

    [Authorize(IdentityClientPermissions.IdentityClient.Update)]
    public virtual async Task<IdentityClientDto> UpdateAsync(Guid id, IdentityClientUpdateDto input)
    {
        var entity = await _identityClientRepository.GetAsync(id);

        entity.DisplayName = input.DisplayName;
        entity.ProviderType = input.ProviderType;
        entity.IsEnabled = input.IsEnabled;
        entity.DisplayOrder = input.DisplayOrder;
        entity.IsDebugMode = input.IsDebugMode;
        entity.RequiredClaimTypes = input.RequiredClaimTypes ?? new List<string>();
        entity.ClaimMaps = input.ClaimMaps?.ConvertAll(x => new IdentityClientClaimMap()
        {
            ClaimType = x.ClaimType,
            Action = x.Action,
            ValueFromType = x.ValueFromType,
            RawValue = x.RawValue,
        }) ?? new List<IdentityClientClaimMap>();

        input.MapExtraPropertiesTo(entity);

        if (input.ProviderType == IdentityProviderType.OpenIdConnect && input.OpenIdConnectConfiguration != null)
        {
            entity.Configurations = IdentityClientConfigurationHelper.ToConfigurations(ObjectMapper.Map<IdentityClientOpenIdConnectConfigurationDto, IdentityClientOpenIdConnectConfiguration>(input.OpenIdConnectConfiguration)).ToList();
        }
        else if (input.ProviderType == IdentityProviderType.Saml2 && input.Saml2Configuration != null)
        {
            entity.Configurations = IdentityClientConfigurationHelper.ToConfigurations(ObjectMapper.Map<IdentityClientSaml2ConfigurationDto, IdentityClientSaml2Configuration>(input.Saml2Configuration)).ToList();
        }

        await _identityClientRepository.UpdateAsync(entity);

        return ObjectMapper.Map<IdentityClient, IdentityClientDto>(entity);
    }

    [Authorize(IdentityClientPermissions.IdentityClient.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _identityClientRepository.DeleteAsync(id);
    }

    public virtual async Task VerifyAsync(Guid id)
    {
        try
        {
            var entity = await _identityClientRepository.GetAsync(id);
            await _identityClientRegisterProvider.ValidateAsync(entity);
        }
        catch (Exception ex)
        {
            throw new BusinessException(IdentityClientErrorCodes.IdentityClientConfigurationIncorrect, details: ex.Message, innerException: ex);
        }
    }
}
