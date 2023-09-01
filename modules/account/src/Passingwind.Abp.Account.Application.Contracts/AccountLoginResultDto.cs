namespace Passingwind.Abp.Account;

public class AccountLoginResultDto
{
    public AccountLoginResultDto(AccountLoginResultType result)
    {
        Result = result;
    }

    public AccountLoginResultType Result { get; }

    public string Description => Result.ToString();
}
