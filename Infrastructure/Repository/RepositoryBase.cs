using System.Collections.Generic;
using System.Data;

namespace Infrastructure.Repository
{
    public abstract class RepositoryBase<T> where T : new()
    {
        private readonly IDataContext _context;

        protected RepositoryBase(IDataContext context)
        {
            _context = context;
        }

        public IDataContext Context
        {
            get { return _context; }
        }

        protected IList<T> ToList(IDbCommand command)
        {
            using (var reader = command.ExecuteReader())
            {
                var items = new List<T>();
                var columnOridinal = GetColumnOrdinal(reader);
                while (reader != null && reader.Read())
                {
                    var item = new T();
                    Map(reader, columnOridinal, item);
                    items.Add(item);
                }
                return items;
            }


        }



        protected abstract void Map(IDataRecord record, Dictionary<string, int> columnsOrdinal, T item);

        protected abstract Dictionary<string, int> GetColumnOrdinal(System.Data.IDataRecord record);
    }
}