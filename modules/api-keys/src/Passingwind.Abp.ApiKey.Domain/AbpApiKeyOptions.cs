using System.Reflection;

namespace Passingwind.Abp.ApiKey;

public class AbpApiKeyOptions
{
    /// <summary>
    ///  Default value: true
    /// </summary>
    public bool ConfigreAuthentication { get; set; } = true;

    /// <summary>
    ///  Default value: <c>Assembly.GetExecutingAssembly().GetName().Name</c>
    /// </summary>
    public string Realm { get; set; }

    public AbpApiKeyOptions()
    {
        Realm = Assembly.GetExecutingAssembly().GetName().Name;
    }
}
