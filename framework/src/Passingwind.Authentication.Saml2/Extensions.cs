using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Passingwind.Authentication.Saml2;

static class Extensions
{
    public static ITfoxtec.Identity.Saml2.Http.HttpRequest ToGenericHttpRequest(this HttpRequest request, bool readBodyAsString = false)
    {
        return new ITfoxtec.Identity.Saml2.Http.HttpRequest
        {
            Method = request.Method,
            QueryString = request.QueryString.Value,
            Query = ToNameValueCollection(request.Query),
            Form = "POST".Equals(request.Method, StringComparison.InvariantCultureIgnoreCase) ? ToNameValueCollection(request.Form) : null
        };
    }
    private static NameValueCollection ToNameValueCollection(IEnumerable<KeyValuePair<string, StringValues>> items)
    {
        var nv = new NameValueCollection();
        foreach (var item in items)
        {
            nv.Add(item.Key, item.Value[0]);
        }
        return nv;
    }

    //private static async Task<string> ReadBodyStringAsync(HttpRequest request)
    //{
    //    using (var reader = new StreamReader(request.Body))
    //    {
    //        return await reader.ReadToEndAsync();
    //    }
    //}
}
