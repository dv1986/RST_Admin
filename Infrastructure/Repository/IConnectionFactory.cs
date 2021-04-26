using System.Data;

namespace Infrastructure.Repository
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}