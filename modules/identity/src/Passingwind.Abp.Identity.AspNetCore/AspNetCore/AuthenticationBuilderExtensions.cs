using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Passingwind.Abp.Identity.AspNetCore;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddChangePasswordCookie(this AuthenticationBuilder builder)
    {
        return builder.AddCookie(IdentityV2Constants.ChangePasswordUserIdScheme, options =>
        {
            options.Cookie.Name = IdentityV2Constants.ChangePasswordUserIdScheme;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        });
    }
}
