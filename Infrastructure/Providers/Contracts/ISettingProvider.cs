namespace Infrastructure.Providers.Contracts;

public interface ISettingProvider
{
    string GetSettingByKey(string key);
    string GetHostName();
}
