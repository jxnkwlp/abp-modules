using Passingwind.Abp.IdentityClient.Identity;

namespace Passingwind.Abp.IdentityClient.Controllers;

public class ExternalLoginCallbackErrorDto
{
    public bool Error { get; set; }
    public ExternalLoginResultType? Result { get; set; }
    public string? Description { get; set; }
}
