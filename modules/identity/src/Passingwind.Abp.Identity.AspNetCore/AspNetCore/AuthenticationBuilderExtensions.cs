using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Passingwind.Abp.Identity.AspNetCore;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddRequiresChangePasswordCookie(this AuthenticationBuilder builder)
    {
        return builder.AddCookie(MyIdentityConstants.RequiresChangePasswordScheme, options =>
        {
            options.Cookie.Name = MyIdentityConstants.RequiresChangePasswordScheme;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        });
    }
}
