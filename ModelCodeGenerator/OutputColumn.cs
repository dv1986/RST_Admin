using System;
using System.Collections.Generic;
using System.Text;

namespace ModelCodeGenerator
{
    public class OutputColumn
    {
        public string HeaderName { get; set; }
        public string ColumnDBType { get; set; }
        public Type ColumnType { get; set; }
        public string Type { get; set; }
        public string Field { get; set; }
    }
}
