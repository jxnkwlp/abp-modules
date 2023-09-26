namespace Passingwind.Abp.Account;

public class Account2FaStateDto
{
    public bool Enabled { get; set; }
    public string[] Providers { get; set; } = null!;
}
