namespace Passingwind.Abp.Account;

public class AccountTFaStateDto
{
    public bool Enabled { get; set; }
    public string[] Providers { get; set; } = null!;
}
