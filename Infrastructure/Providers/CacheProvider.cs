using AutoDependencyRegistration.Attributes;
using Domain.Commons;
using Infrastructure.Providers.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Providers;

[RegisterClassAsScoped]
public class CacheProvider : ICacheProvider
{
    protected string CacheKey = typeof(BaseEntity).Name;
    protected string CachePublishKey = typeof(BaseVersion).Name;
    protected IMemoryCache _memoryCache;
    public CacheProvider(IMemoryCache memoryCache) => _memoryCache = memoryCache;
    public void Reset()
    {
        List<string> result = new();
        IEnumerable<Type> baseTypes = typeof(BaseEntity).Assembly.GetExportedTypes().Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(BaseEntity).IsAssignableFrom(c));
        foreach (var type in baseTypes)
        {
            if (_memoryCache.Get(type.Name) != null)
                _memoryCache.Remove(type.Name);
        }
        IEnumerable<Type> versionTypes = typeof(BaseVersion).Assembly.GetExportedTypes().Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(BaseVersion).IsAssignableFrom(c));
        foreach (var type in versionTypes)
        {
            if (_memoryCache.Get(type.Name) != null)
                _memoryCache.Remove(type.Name);
        }
    }
    public TItem Set<TItem>(string key, TItem value) => _memoryCache.Set(key, value, TimeSpan.FromHours(12));
    public bool Get<TItem>(object key, out TItem value) => _memoryCache.TryGetValue(key, out value);


}
