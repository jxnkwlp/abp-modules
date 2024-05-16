using System;
using System.Collections.Generic;

namespace Passingwind.Abp.Account.Events;

[Serializable]
public class UserLoginEvent
{
    public const string PasswordLogin = "PasswordLogin";
    public const string AuthenticatorRecoveryCodeLogin = "AuthenticatorRecoveryCodeLogin";
    public const string TwoFactorLogin = "TwoFactorLogin";
    public const string Logout = "Logout";
    public const string ExternalLogin = "ExternalLogin";
    public const string ImpersonationLogin = "ImpersonationLogin";
    public const string ImpersonationLogout = "ImpersonationLogout";
    public const string LinkLogin = "LinkLogin";
    public const string DelegationLogin = "DelegationLogin";

    public UserLoginEvent(Guid userId, string @event, Dictionary<string, object>? eventData = null)
    {
        UserId = userId;
        Event = @event;
        EventData = eventData ?? new Dictionary<string, object>();
    }

    public Guid UserId { get; set; }

    public string Event { get; set; } = null!;

    public Dictionary<string, Object> EventData { get; }
}
