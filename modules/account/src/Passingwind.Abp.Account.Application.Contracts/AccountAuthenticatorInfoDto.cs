﻿namespace Passingwind.Abp.Account;

public class AccountAuthenticatorInfoDto
{
    public bool Enabled { get; set; }
    public string? Identifier { get; set; }
    public string? Key { get; set; }
    public string? FormatKey { get; set; }
    public string? Uri { get; set; }
}
