namespace Passingwind.Abp.IdentityClientManagement.Options;

public class IdentityClientOption
{
    /// <summary>
    ///  Default: true
    /// </summary>
    public bool ConfigureAuthenticationSchame { get; set; } = true;

    public string? ErrorPageUrl { get; set; } = "/error";

    /// <summary>
    ///  Default: false
    /// </summary>
    public bool RedirectToErrorPage { get; set; }

    /// <summary>
    ///  Default: true
    /// </summary>
    public bool GenerateUserName { get; set; } = true;

    public string? SigningCertificateFilePath { get; set; }
}
