using System.Data;

namespace Infrastructure.Repository
{
    public interface IConnectionFactoryCache
    {
        IDbConnection CreateConnection();
    }
}