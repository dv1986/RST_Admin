using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Grid
{
    public class GridSummary
    {
        public string ColumnName { get; set; }
        public string Field { get; set; }
        public decimal AggVal { get; set; }
        public string Aggregate { get; set; }
    }
}
