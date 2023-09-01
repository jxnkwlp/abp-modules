using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Passingwind.Abp.Identity.AspNetCore;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddApplicationPartialCookie(this AuthenticationBuilder builder)
    {
        return builder.AddCookie(MyIdentityConstants.ApplicationPartialScheme, options =>
        {
            options.Cookie.Name = MyIdentityConstants.ApplicationPartialScheme;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        });
    }
}
