using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Pivot
{
    public class PivotCol
    {
        public string Field { get; set; }
        public string Column { get; set; }
        public string DataType { get; set; } /*Possible values int,string,decimal*/
    }
}
