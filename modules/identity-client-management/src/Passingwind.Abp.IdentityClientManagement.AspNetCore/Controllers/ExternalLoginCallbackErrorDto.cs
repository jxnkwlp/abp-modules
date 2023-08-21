using Passingwind.Abp.IdentityClientManagement.Identity;

namespace Passingwind.Abp.IdentityClientManagement.Controllers;

public class ExternalLoginCallbackErrorDto
{
    public bool Error { get; set; }
    public ExternalLoginResultType? Result { get; set; }
    public string? Description { get; set; }
}
