using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Pivot
{
    public class PivotColumn
    {
        public string aggFunc { get; set; }
        public string displayName { get; set; }
        public string field { get; set; }
        public string id { get; set; }

    }
}
