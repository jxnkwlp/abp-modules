namespace Passingwind.Abp.Account;

public class AccountExternalLoginCallbackDto
{
    public string? ReturnUrl { get; set; }
    public string? RemoteError { get; set; }
}
