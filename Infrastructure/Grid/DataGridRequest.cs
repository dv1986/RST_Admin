using System;
using System.Collections.Generic;

namespace Infrastructure.Grid
{
    public class DataGridRequest<TQuery>
    {
        public List<ColumnHeader> ColumnHeaders { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public List<SortColumn> SortColumns { get; set; }

        public List<ColumnFilter> ColumnFilters { get; set; }
        /// <summary>
        /// Data Retrival Query which contains all conditions, and Sorting operations.
        /// </summary>
        public TQuery RetrivalQuery { get; set; }

        public string GridGuid { get; set; }


    }
}