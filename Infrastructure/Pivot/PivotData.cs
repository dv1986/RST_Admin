using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Pivot
{
    public class PivotData
    {
        public DataTable Data { get; set; }
        public List<string> SecondaryColumns { get; set; }
    }
}
