using Microsoft.Extensions.Caching.Memory;
using NotificationService.Core.Services;

namespace NotificationService.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void Set<T>(string key, T value, TimeSpan absoluteExpirationRelativeToNow)
        {
            _cache.Set(key, value, absoluteExpirationRelativeToNow);
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }
    }
}
