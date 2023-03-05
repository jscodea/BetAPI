using Microsoft.Extensions.Caching.Memory;

namespace BetAPI.Services
{
    public class AppCache: IAppCache
    {
        public MemoryCache Cache { get; } = new MemoryCache(
        new MemoryCacheOptions
        {
            SizeLimit = 1024
        });
    }
}
