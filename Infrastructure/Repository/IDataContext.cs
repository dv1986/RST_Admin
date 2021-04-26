using System.Data;
using Cos.BCS.Infrastructure.UnitOfWork;

namespace Infrastructure.Repository
{
    public interface IDataContext
    {
        IUnitOfWork CreateUnitOfWork();
        IDbCommand CreateCommand();

        IDbConnection Connection { get; }
    }
}