using Infrastructure.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Pivot
{
    public class PivotRequestModel
    {
        public int startRow { get; set; }
        public int endRow { get; set; }
        public List<PivotColumn> rowGroupCols { get; set; }
        public List<PivotColumn> valueCols { get; set; }
        public List<PivotColumn> pivotCols { get; set; }

        public bool pivotMode { get; set; }
        public List<string> groupKeys { get; set; }
        public List<ColumnFilter> filterModel { get; set; }

        public List<SortColumn> sortModel { get; set; }
    }
}
