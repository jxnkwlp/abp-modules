using Microsoft.AspNetCore.Http;
using Passingwind.AspNetCore.Authentication.ApiKey;

namespace Passingwind.Abp.ApiKey;

public static class HttpContextExtensions
{
    public static bool IsApiKeyAuthorizationRequest(
        this HttpContext context,
        string queryName = ApiKeyDefaults.QueryStringName,
        string headerName = ApiKeyDefaults.HeaderName,
        string headerAuthenticationSchemeName = ApiKeyDefaults.HeaderAuthenticationSchemeName)
    {
        var authorization = (string?)context.Request.Headers.Authorization;
        if (authorization?.StartsWith(headerAuthenticationSchemeName) == true)
        {
            return true;
        }

        if (context.Request.Headers.ContainsKey(headerName))
        {
            return true;
        }

        if (context.Request.Query.ContainsKey(queryName))
        {
            return true;
        }

        return false;
    }
}
