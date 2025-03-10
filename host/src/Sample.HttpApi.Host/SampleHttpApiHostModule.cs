using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Passingwind.Abp.Account;
using Passingwind.Abp.ApiKey;
using Passingwind.Abp.FileManagement;
using Passingwind.Abp.FileManagement.Options;
using Passingwind.Abp.PermissionManagement;
using Passingwind.AspNetCore.Authentication.ApiKey;
using Sample.EntityFrameworkCore;
using Sample.MultiTenancy;
using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.Libs;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.BackgroundWorkers.Hangfire;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.Uow;
using Volo.Abp.VirtualFileSystem;

namespace Sample;

[DependsOn(
    typeof(SampleHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(SampleApplicationModule),
    typeof(SampleEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpOpenIddictAspNetCoreModule),
    typeof(AbpBackgroundJobsHangfireModule),
    typeof(AbpBackgroundWorkersHangfireModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
[DependsOn(typeof(ApiKeyAspNetCoreModule))]
[DependsOn(typeof(FileManagementApplicationModule))]
[DependsOn(typeof(AccountAspNetCoreIdentityClientModule))]
[DependsOn(typeof(AccountAspNetCoreModule))]
[DependsOn(typeof(PermissionManagementHttpApiModule))]
public class SampleHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("Sample");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureAuthentication(context);
        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);

        //context.Services.AddAlwaysAllowAuthorization();

        Configure<AbpBlobStoringOptions>(options => options.Containers.ConfigureDefault(container => container.UseFileSystem(fileSystem => fileSystem.BasePath = "C:\\my-files")));

        Configure<FileManagementOptions>(options =>
        {
            options.DefaultOverrideBehavior = FileOverrideBehavior.Rename;
            options.DefaultContainerAccessMode = FileAccessMode.Authorized;
        });

        context.Services.AddHangfire(config =>
        {
            config.UseMemoryStorage();

            config
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseColouredConsoleLogProvider();
        });

        Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Lax;
            options.Secure = CookieSecurePolicy.SameAsRequest;
        });

        Configure<AbpAntiForgeryOptions>(options => options.TokenCookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax);

        Configure<AbpMvcLibsOptions>(options => options.CheckLibs = false);
        
        Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Auto;
            // options.IsolationLevel = System.Data.IsolationLevel.ReadUncommitted;
        });

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = "./tmp/";
                });
            });
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        //context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        //context.Services.ForwardIdentityAuthenticationForApiKey();

        context.Services.ConfigureApplicationCookie(options =>
        {
            options.ForwardDefaultSelector = ctx =>
            {
                string? authorization = ctx.Request.Headers.Authorization;
                if (!string.IsNullOrWhiteSpace(authorization) && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    return "Bearer";
                }

                if (ctx.IsApiKeyAuthorizationRequest())
                {
                    return ApiKeyDefaults.AuthenticationScheme;
                }

                return null;
            };
        });
    }

    private void ConfigureBundles()
    {

        //Configure<AbpBundlingOptions>(options =>
        //{
        //    options.StyleBundles.Configure(
        //        LeptonXLiteThemeBundles.Styles.Global,
        //        bundle =>
        //        {
        //            bundle.AddFiles("/global-styles.css");
        //        }
        //    );
        //});
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<SampleDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Sample.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<SampleDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Sample.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<SampleApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Sample.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<SampleApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Sample.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options => options.ConventionalControllers.Create(typeof(SampleApplicationModule).Assembly));
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        var authority = configuration["AuthServer:Authority"];
        if (string.IsNullOrWhiteSpace(authority))
            return;

        context.Services.Replace(ServiceDescriptor.Transient<ISwaggerHtmlResolver, SwaggerHtmlResolver>());
        context.Services.AddAbpSwaggerGenWithOAuth(
            authority,
            new Dictionary<string, string>
            {
                    {"Sample", "Sample API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Sample API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.ApplyExtensions();
                options.OrderActionsBy(x => x.GroupName + x.RelativePath);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseHangfireDashboard();

        //app.UseUnitOfWork();
        app.UseAuthorization();

        app.UseSwagger();
        // var resolver = app.ApplicationServices.GetService<ISwaggerHtmlResolver>();
        app.UseAbpSwaggerUI(c =>
        {
            //c.InjectJavascript("ui/abp.js");
            c.InjectJavascript("ui/abp.swagger.js");
            //// c.IndexStream = () => new MemoryStream(Encoding.UTF8.GetBytes(new StreamReader(resolver!!?.Resolver()).ReadToEnd().Replace("src=\"index.js\"", "src=\"ui/index.js\"")));
            //c.IndexStream = () => resolver?.Resolver();

            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample API");
            c.DisplayOperationId();
            c.DisplayRequestDuration();
            c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            c.ConfigObject.AdditionalItems.Add("tagsSorter", "alpha");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}

//public class SwaggerHtmlResolver : ISwaggerHtmlResolver, ITransientDependency
//{
//    public virtual Stream Resolver()
//    {
//        var stream = typeof(SwaggerUIOptions).GetTypeInfo().Assembly
//            .GetManifestResourceStream("Swashbuckle.AspNetCore.SwaggerUI.index.html");

//        var html = new StreamReader(stream!)
//            .ReadToEnd()
//            .Replace("src=\"index.js\"", "src=\"ui/index.js\"");

//        return new MemoryStream(Encoding.UTF8.GetBytes(html));
//    }
//}
