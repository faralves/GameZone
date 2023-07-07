using GameZone.Core.DomainObjects;
using Microsoft.Extensions.Configuration;

namespace GameZone.Core.ConfigApp;

public class ConfigParameters : IConfigParameters
{
    private readonly IConfiguration _configuration;

    public ConfigParameters(IConfiguration configuration) 
    {
        _configuration = configuration;
    }

    public void EnableConnectionLocal(bool enabled)
    {
        GeneralConfigApp.ENABLE_CONNECTION_LOCAL_DB = enabled;
    }

    public void SetGeneralConfig()
    {
        var connectionParameter = _configuration["EnableLocalExecution"];
        var enableConnectionLocal = Boolean.Parse(connectionParameter);
        GeneralConfigApp.ENABLE_CONNECTION_LOCAL_DB = enableConnectionLocal;
    }
}
