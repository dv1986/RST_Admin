using System.Data;
using Cos.BCS.Infrastructure.UnitOfWork;

namespace Infrastructure.Repository
{
    public interface IDataContextCache
    {
        IUnitOfWork CreateUnitOfWork();
        IDbCommand CreateCommand();

        IDbConnection Connection { get; }
    }
}