using Infrastructure.Repository;

namespace Cos.CosBCS.Infrastructure.Query
{
    public abstract class QueryHandlerBase
    {
        private readonly IDataContext _context;

        protected QueryHandlerBase(IDataContext context)
        {
            _context = context;
        }

        public IDataContext Context
        {
            get { return _context; }
        }
    }
}