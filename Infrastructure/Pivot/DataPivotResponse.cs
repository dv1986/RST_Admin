using Infrastructure.Grid;
using RST.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Pivot
{

    public class DataPivotResponse<TData>
    {
        public List<ColumnMetaData> HeaderMetaData { get; set; }
        public int TotalDataCount { get; set; }

        public TData Data { get; set; }

        public string GridId { get; set; }

        public List<string> SecondaryColumns { get; set; }

        public ResponseState State { get; set; } = ResponseState.Success;

        public List<string> Messages { get; set; } = new List<string>();
    }
}
