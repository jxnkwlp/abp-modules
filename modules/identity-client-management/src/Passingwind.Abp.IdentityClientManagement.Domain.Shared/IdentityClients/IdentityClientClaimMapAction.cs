namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public enum IdentityClientClaimMapAction
{
    /// <summary>
    ///  Add an claim type from value type or raw value if not exists
    /// </summary>
    AddIfNotExists = 0,
    /// <summary>
    ///  Add or update an claim type from value type or raw value
    /// </summary>
    AddOrUpdate = 1,
    /// <summary>
    ///  Append an claim type from value type or raw value
    /// </summary>
    Append = 2,
    /// <summary>
    ///  Remove an exist claim type from value type
    /// </summary>
    Remove = 3
}
