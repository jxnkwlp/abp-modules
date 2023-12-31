﻿using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountVerifyTokenRequestDto
{
    [Required]
    public string Token { get; set; } = null!;
}
