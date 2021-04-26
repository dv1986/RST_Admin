using System.Collections.Generic;

namespace Infrastructure.Query
{
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQuery<TResult>
    {
        TResult FindBy(TQuery query, out int totalRecords);
    }
}