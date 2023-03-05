using Microsoft.Extensions.Caching.Memory;

namespace BetAPI.Services
{
    public interface IAppCache
    {
        public MemoryCache Cache { get; }
    }
}
