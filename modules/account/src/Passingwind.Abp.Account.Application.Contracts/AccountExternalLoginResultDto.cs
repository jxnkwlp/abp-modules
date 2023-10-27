namespace Passingwind.Abp.Account;

public class AccountExternalLoginResultDto : AccountLoginResultDto
{
    public AccountExternalLoginResultDto(AccountLoginResultType result) : base(result)
    {
    }

    public string? RedirectUrl { get; set; }
}
