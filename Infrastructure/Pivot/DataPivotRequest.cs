
using Infrastructure.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Pivot
{
    public class DataPivotRequest<TQuery>
    {
        public List<ColumnHeader> ColumnHeaders { get; set; }
        public PivotRequestModel RequestModel { get; set; }
        public List<FilterParameter> Filters { get; set; }
        public string GridGuid { get; set; }
        public TQuery RetrievalQuery { get; set; }
        public string ProcedureName { get; set; }
        public List<PivotCol> Columns { get; set; }
        public string AlwaysGroupOnColumn { get; set; }
    }
}
