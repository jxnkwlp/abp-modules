using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.Abp.Localization.External;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Volo.Abp.Users;
using TimeZone = Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.TimeZone;

namespace Passingwind.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class ApplicationConfigurationAppService : ApplicationService, IApplicationConfigurationAppService
{
    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected AbpMultiTenancyOptions MultiTenancyOptions { get; }
    protected AbpClockOptions ClockOptions { get; }
    protected AbpApplicationConfigurationOptions ApplicationConfigurationOptions { get; }
    protected ApplicationConfigurationEndpointOptions ApplicationConfigurationEndpointOptions { get; }
    protected IAbpAuthorizationPolicyProvider AbpAuthorizationPolicyProvider { get; }
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
    protected DefaultAuthorizationPolicyProvider DefaultAuthorizationPolicyProvider { get; }
    protected IPermissionChecker PermissionChecker { get; }
    protected ISettingDefinitionManager SettingDefinitionManager { get; }
    protected IFeatureDefinitionManager FeatureDefinitionManager { get; }
    protected ILanguageProvider LanguageProvider { get; }
    protected ITimezoneProvider TimezoneProvider { get; }
    protected ICachedObjectExtensionsDtoService CachedObjectExtensionsDtoService { get; }

    public ApplicationConfigurationAppService(
        IOptions<AbpLocalizationOptions> localizationOptions,
        IOptions<AbpMultiTenancyOptions> multiTenancyOptions,
        IOptions<AbpClockOptions> clockOptions,
        IOptions<AbpApplicationConfigurationOptions> applicationConfigurationOptions,
        IOptions<ApplicationConfigurationEndpointOptions> applicationConfigurationEndpointOptions,
        IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider,
        IPermissionDefinitionManager permissionDefinitionManager,
        DefaultAuthorizationPolicyProvider defaultAuthorizationPolicyProvider,
        IPermissionChecker permissionChecker,
        ISettingDefinitionManager settingDefinitionManager,
        IFeatureDefinitionManager featureDefinitionManager,
        ILanguageProvider languageProvider,
        ITimezoneProvider timezoneProvider,
        ICachedObjectExtensionsDtoService cachedObjectExtensionsDtoService
        )
    {
        LocalizationOptions = localizationOptions.Value;
        MultiTenancyOptions = multiTenancyOptions.Value;
        ClockOptions = clockOptions.Value;
        ApplicationConfigurationOptions = applicationConfigurationOptions.Value;
        ApplicationConfigurationEndpointOptions = applicationConfigurationEndpointOptions.Value;
        AbpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
        PermissionDefinitionManager = permissionDefinitionManager;
        DefaultAuthorizationPolicyProvider = defaultAuthorizationPolicyProvider;
        PermissionChecker = permissionChecker;
        SettingDefinitionManager = settingDefinitionManager;
        FeatureDefinitionManager = featureDefinitionManager;
        LanguageProvider = languageProvider;
        TimezoneProvider = timezoneProvider;
        CachedObjectExtensionsDtoService = cachedObjectExtensionsDtoService;
    }

    public virtual async Task<ApplicationConfigurationDto> GetAsync(ApplicationConfigurationRequestOptions options)
    {
        Logger.LogDebug("Executing AbpApplicationConfigurationAppService.GetAsync()...");

        var result = new ApplicationConfigurationDto
        {
            Auth = ApplicationConfigurationEndpointOptions.EnableAuth ? await GetAuthConfigAsync() : default!,
            Features = ApplicationConfigurationEndpointOptions.EnableFeatures ? await GetFeaturesConfigAsync() : default!,
            GlobalFeatures = ApplicationConfigurationEndpointOptions.EnableGlobalFeatures ? await GetGlobalFeaturesConfigAsync() : default!,
            Localization = ApplicationConfigurationEndpointOptions.EnableLocalization ? await GetLocalizationConfigAsync(options) : default!,
            CurrentUser = ApplicationConfigurationEndpointOptions.EnableCurrentUser ? GetCurrentUser() : default!,
            Setting = ApplicationConfigurationEndpointOptions.EnableSetting ? await GetSettingConfigAsync() : default!,
            MultiTenancy = ApplicationConfigurationEndpointOptions.EnableMultiTenancy ? GetMultiTenancy() : default!,
            CurrentTenant = ApplicationConfigurationEndpointOptions.EnableCurrentTenant ? GetCurrentTenant() : default!,
            Timing = ApplicationConfigurationEndpointOptions.EnableTiming ? await GetTimingConfigAsync() : default!,
            Clock = ApplicationConfigurationEndpointOptions.EnableClock ? GetClockConfig() : default!,
            ObjectExtensions = ApplicationConfigurationEndpointOptions.EnableObjectExtensions ? CachedObjectExtensionsDtoService.Get() : default!,
            ExtraProperties = new ExtraPropertyDictionary()
        };

        if (ApplicationConfigurationOptions.Contributors.Any())
        {
            using (var scope = LazyServiceProvider.CreateScope())
            {
                var context = new ApplicationConfigurationContributorContext(scope.ServiceProvider, result);
                foreach (var contributor in ApplicationConfigurationOptions.Contributors)
                {
                    await contributor.ContributeAsync(context);
                }
            }
        }

        Logger.LogDebug("Executed AbpApplicationConfigurationAppService.GetAsync().");

        return result;
    }

    protected virtual CurrentTenantDto GetCurrentTenant()
    {
        return new CurrentTenantDto()
        {
            Id = CurrentTenant.Id,
            Name = CurrentTenant.Name!,
            IsAvailable = CurrentTenant.IsAvailable
        };
    }

    protected virtual MultiTenancyInfoDto GetMultiTenancy()
    {
        return new MultiTenancyInfoDto
        {
            IsEnabled = MultiTenancyOptions.IsEnabled
        };
    }

    protected virtual CurrentUserDto GetCurrentUser()
    {
        return new CurrentUserDto
        {
            IsAuthenticated = CurrentUser.IsAuthenticated,
            Id = CurrentUser.Id,
            TenantId = CurrentUser.TenantId,
            ImpersonatorUserId = CurrentUser.FindImpersonatorUserId(),
            ImpersonatorTenantId = CurrentUser.FindImpersonatorTenantId(),
            ImpersonatorUserName = CurrentUser.FindImpersonatorUserName(),
            ImpersonatorTenantName = CurrentUser.FindImpersonatorTenantName(),
            UserName = CurrentUser.UserName,
            SurName = CurrentUser.SurName,
            Name = CurrentUser.Name,
            Email = CurrentUser.Email,
            EmailVerified = CurrentUser.EmailVerified,
            PhoneNumber = CurrentUser.PhoneNumber,
            PhoneNumberVerified = CurrentUser.PhoneNumberVerified,
            Roles = CurrentUser.Roles,
            SessionId = CurrentUser.FindSessionId()
        };
    }

    protected virtual async Task<ApplicationAuthConfigurationDto> GetAuthConfigAsync()
    {
        var authConfig = new ApplicationAuthConfigurationDto();

        var policyNames = await AbpAuthorizationPolicyProvider.GetPoliciesNamesAsync();
        var abpPolicyNames = new List<string>();
        var otherPolicyNames = new List<string>();

        foreach (var policyName in policyNames)
        {
            if (await DefaultAuthorizationPolicyProvider.GetPolicyAsync(policyName) == null &&
                await PermissionDefinitionManager.GetOrNullAsync(policyName) != null)
            {
                abpPolicyNames.Add(policyName);
            }
            else
            {
                otherPolicyNames.Add(policyName);
            }
        }

        foreach (var policyName in otherPolicyNames)
        {
            if (await AuthorizationService.IsGrantedAsync(policyName))
            {
                authConfig.GrantedPolicies[policyName] = true;
            }
        }

        var result = await PermissionChecker.IsGrantedAsync(abpPolicyNames.ToArray());
        foreach (var (key, value) in result.Result)
        {
            if (value == PermissionGrantResult.Granted)
            {
                authConfig.GrantedPolicies[key] = true;
            }
        }

        return authConfig;
    }

    protected virtual async Task<ApplicationLocalizationConfigurationDto> GetLocalizationConfigAsync(
        ApplicationConfigurationRequestOptions options)
    {
        var localizationConfig = new ApplicationLocalizationConfigurationDto();

        localizationConfig.Languages.AddRange(await LanguageProvider.GetLanguagesAsync());

        if (options.IncludeLocalizationResources)
        {
            var resourceNames = LocalizationOptions
                .Resources
                .Values
                .Select(x => x.ResourceName)
                .Union(
                    await LazyServiceProvider
                        .LazyGetRequiredService<IExternalLocalizationStore>()
                        .GetResourceNamesAsync()
                );

            foreach (var resourceName in resourceNames)
            {
                var dictionary = new Dictionary<string, string>();

                var localizer = await StringLocalizerFactory
                    .CreateByResourceNameOrNullAsync(resourceName);

                if (localizer != null)
                {
                    foreach (var localizedString in await localizer.GetAllStringsAsync())
                    {
                        dictionary[localizedString.Name] = localizedString.Value;
                    }
                }

                localizationConfig.Values[resourceName] = dictionary;
            }
        }

        localizationConfig.CurrentCulture = GetCurrentCultureInfo();

        if (LocalizationOptions.DefaultResourceType != null)
        {
            localizationConfig.DefaultResourceName = LocalizationResourceNameAttribute.GetName(
                LocalizationOptions.DefaultResourceType
            );
        }

        localizationConfig.LanguagesMap = LocalizationOptions.LanguagesMap;
        localizationConfig.LanguageFilesMap = LocalizationOptions.LanguageFilesMap;

        return localizationConfig;
    }

    private static CurrentCultureDto GetCurrentCultureInfo()
    {
        return CurrentCultureDto.Create();
    }

    private async Task<ApplicationSettingConfigurationDto> GetSettingConfigAsync()
    {
        var result = new ApplicationSettingConfigurationDto
        {
            Values = new Dictionary<string, string>()
        };

        var settingDefinitions = (await SettingDefinitionManager.GetAllAsync()).Where(x => x.IsVisibleToClients);

        var settingValues = await SettingProvider.GetAllAsync(settingDefinitions.Select(x => x.Name).ToArray());

        foreach (var settingValue in settingValues)
        {
            result.Values[settingValue.Name] = settingValue.Value;
        }

        return result;
    }

    protected virtual async Task<ApplicationFeatureConfigurationDto> GetFeaturesConfigAsync()
    {
        var result = new ApplicationFeatureConfigurationDto();

        foreach (var featureDefinition in await FeatureDefinitionManager.GetAllAsync())
        {
            if (!featureDefinition.IsVisibleToClients)
            {
                continue;
            }

            result.Values[featureDefinition.Name] = await FeatureChecker.GetOrNullAsync(featureDefinition.Name);
        }

        return result;
    }

    protected virtual Task<ApplicationGlobalFeatureConfigurationDto> GetGlobalFeaturesConfigAsync()
    {
        var result = new ApplicationGlobalFeatureConfigurationDto();

        foreach (var enabledFeatureName in GlobalFeatureManager.Instance.GetEnabledFeatureNames())
        {
            result.EnabledFeatures.AddIfNotContains(enabledFeatureName);
        }

        return Task.FromResult(result);
    }

    protected virtual async Task<TimingDto> GetTimingConfigAsync()
    {
        var windowsTimeZoneId = await SettingProvider.GetOrNullAsync(TimingSettingNames.TimeZone);

        return new TimingDto
        {
            TimeZone = new TimeZone
            {
                Windows = new WindowsTimeZone
                {
                    TimeZoneId = windowsTimeZoneId
                },
                Iana = new IanaTimeZone
                {
                    TimeZoneName = windowsTimeZoneId.IsNullOrWhiteSpace()
                        ? null
                        : TimezoneProvider.WindowsToIana(windowsTimeZoneId!)
                }
            }
        };
    }

    protected virtual ClockDto GetClockConfig()
    {
        return new ClockDto
        {
            Kind = Enum.GetName(typeof(DateTimeKind), ClockOptions.Kind)!
        };
    }
}
