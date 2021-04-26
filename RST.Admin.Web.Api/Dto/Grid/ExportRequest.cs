using Infrastructure.Grid;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RST.Admin.Web.Api.Dto.Grid
{
    public class ExportRequest
    {
        public string CacheId { get; set; }

        public List<ExportColumn> ColumnHeaders { get; set; }

        public ICollection<dynamic> LocalData { get; set; }

        public List<ColumnFilter> ColumnFilters { get; set; }

        public List<SortColumn> SortColumns { get; set; }
    }
}