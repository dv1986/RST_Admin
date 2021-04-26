using System;
using System.Collections.Generic;
using RST.Shared.Enums;

namespace Infrastructure.Grid
{
    public class DataGridResponse<TData>
    {
        public List<ColumnMetaData> HeaderMetaData { get; set; }

        public int TotlaDataCount { get; set; }
        
        /// <summary>
        /// The resturned data to fill in the grid
        /// </summary>
        public TData Data { get; set; }

        public string GridGuid { get; set; }

        public ResponseState State { get; set; } = ResponseState.Success;

        public List<string> Messages { get; set; } = new List<string>();

        public List<GridSummary> Summary { get; set; }
    }
}