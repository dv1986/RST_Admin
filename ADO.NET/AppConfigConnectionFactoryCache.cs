using Infrastructure.Repository;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace ADO.NET
{
    public class AppConfigConnectionFactoryCache : IConnectionFactoryCache
    {
        private readonly DbProviderFactory _provider;
        private readonly string _connectionString;
        private readonly string _providerName;


        /// <summary>
        /// Initializes a new instance of the <see cref="AppConfigConnectionFactory"/> class.
        /// </summary>
        /// <param name="connectionKey">The connection key.</param>
        /// <exception cref="System.ArgumentNullException">connectionKey</exception>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">Configuration Key not exist</exception>
        public AppConfigConnectionFactoryCache(string connectionKey)
        {
            if (string.IsNullOrEmpty(connectionKey))
                throw new ArgumentNullException("connectionKey");
            var connectionValue = ConfigurationManager.ConnectionStrings[connectionKey];
            if (connectionValue == null)
                throw new ConfigurationErrorsException(string.Format(
                    "Failed to find connection string named '{0}' in app/web.config.",
                    connectionKey));
            _providerName = connectionValue.ProviderName;
            _provider = DbProviderFactories.GetFactory(_providerName);
            _connectionString = connectionValue.ConnectionString;

        }

        public AppConfigConnectionFactoryCache(string connectionString, string providerName)
        {
            _providerName = providerName;
            _provider = DbProviderFactories.GetFactory(_providerName);
            _connectionString = connectionString;
        }
        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException"></exception>
        public IDbConnection CreateConnection()
        {
            var connection = _provider.CreateConnection();
            if (connection == null)
                throw new ConfigurationErrorsException(string.Format(
                    "Failed to find connection string named '{0}' in app/web.config.",
                    _providerName));
            connection.ConnectionString = _connectionString;
            //Delay opening the connection until used
            // connection.Open();

            return connection;

        }
    }
}