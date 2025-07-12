namespace Infrastructure.Providers.Contracts;

public interface ICacheProvider
{
    void Reset();
    TItem Set<TItem>(string key, TItem value);
    bool Get<TItem>(object key, out TItem value);
}
