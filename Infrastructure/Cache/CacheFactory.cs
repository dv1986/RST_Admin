namespace Infrastructure.Cache
{
    public class CacheFactory
    {
        /// <summary>
        /// The _cache storage
        /// </summary>
        private static ICacheStorage _cacheStorage;

        /// <summary>
        /// Initializes the cache factory.[ Uses constructor dependency injection ]
        /// </summary>
        /// <param name="cacheStorage">The cache storage.</param>
        public static void InitializeCacheFactory(ICacheStorage cacheStorage)
        {
            _cacheStorage = cacheStorage;
        }

        /// <summary>
        /// Gets the application cache.
        /// </summary>
        /// <returns>The cache object used</returns>
        public static ICacheStorage GetApplicationCache()
        {
            return _cacheStorage;
        }
    }
}