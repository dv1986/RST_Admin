using System.Collections.Generic;

namespace Infrastructure.Grid
{
    public class ColumnFilter
    {
        public string FieldName { get; set; }

        public List<string> Filter { get; set; }
        
        public string FilterType { get; set; }
        
        public string Operation { get; set; }
        
    }
}