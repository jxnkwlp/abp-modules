namespace Passingwind.Abp.Identity.Options;

public class IdentityUserTokenOptions
{
    /// <summary>
    /// Default: true
    /// </summary>
    public bool RequireConfirmedEmail { get; set; } = true;
    /// <summary>
    /// Default: true
    /// </summary>
    public bool RequireConfirmedPhoneNumber { get; set; } = true;
}
