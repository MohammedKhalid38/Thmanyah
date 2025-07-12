using AutoDependencyRegistration.Attributes;
using Infrastructure.Providers.Contracts;
using Infrastructure.Utilities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Providers;

[RegisterClassAsScoped]
public class SettingProvider : ISettingProvider
{
    private readonly IConfiguration _configuration;
    public SettingProvider(IConfiguration configuration) => _configuration = configuration;
    public string GetSettingByKey(string key) => _configuration[key] ?? string.Empty;
    public string GetHostName() => GetSettingByKey(ApplicationSettings.WebsiteLink);
}
