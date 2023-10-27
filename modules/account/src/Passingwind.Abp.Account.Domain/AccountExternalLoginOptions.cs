namespace Passingwind.Abp.Account;

public class AccountExternalLoginOptions
{
    /// <summary>
    ///  Default: true
    /// </summary>
    public bool RedirectToErrorPage { get; set; } = true;

    /// <summary>
    ///  Default: /error
    /// </summary>
    public string ErrorPageUrl { get; set; } = "/error";
    /// <summary>
    ///  Default: false
    /// </summary>
    public bool LogClaims { get; set; }
}
