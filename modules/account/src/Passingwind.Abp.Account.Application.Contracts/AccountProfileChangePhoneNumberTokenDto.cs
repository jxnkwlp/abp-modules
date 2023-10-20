using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountProfileChangePhoneNumberTokenDto
{
    [Required]
    [MaxLength(16)]
    public string PhoneNumber { get; set; } = null!;
}
