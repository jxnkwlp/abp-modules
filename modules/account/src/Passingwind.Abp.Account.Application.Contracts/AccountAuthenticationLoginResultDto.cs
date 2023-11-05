namespace Passingwind.Abp.Account;

public class AccountAuthenticationLoginResultDto
{
    public string LoginProvider { get; set; } = null!;
    public string? ProviderDisplayName { get; set; }
}
