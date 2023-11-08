using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.IdentityClient.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Area(IdentityClientRemoteServiceConsts.ModuleName)]
[Route("auth/saml2")]
public class Saml2Controller : AbpController
{
    protected ISaml2AppService Saml2AppService { get; }

    public Saml2Controller(ISaml2AppService saml2AppService)
    {
        Saml2AppService = saml2AppService;
    }

    [AllowAnonymous]
    [HttpGet("{name}/metadata")]
    [HttpGet("{name}/endpoint/descriptor")]
    public virtual async Task<IActionResult> Saml2MetadataAsync(string name)
    {
        var url = $"{Request.Scheme}://{Request.Host.ToUriComponent()}{Request.PathBase}";
        var baseUri = new Uri(url.EnsureEndsWith('/'));

        var metadataString = await Saml2AppService.GetMetadataDescriptorAsync(baseUri, name);

        return Content(metadataString, "text/xml", Encoding.UTF8);
    }

    [Authorize]
    [HttpPost("{name}/logout")]
    public virtual async Task<IActionResult> Logout(string name)
    {
        var result = await Saml2AppService.LogoutAsync(name);

        return Content(result, "text/xml", Encoding.UTF8);
    }
}
