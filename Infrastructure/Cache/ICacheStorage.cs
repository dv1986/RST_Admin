namespace Infrastructure.Cache
{
    public interface ICacheStorage
    {
        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Remove(string key);
        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="data">The data to store in cache.</param>
        void Store(string key, object data);
        /// <summary>
        /// Retrieves the specified storage key from the cache.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve </typeparam>
        /// <param name="storageKey">The cache storage key.</param>
        /// <returns>The stored object in the cache</returns>
        T Retrieve<T>(string storageKey);
    }
}