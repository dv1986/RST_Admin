using System;
using System.Collections.Generic;
using System.Text;

namespace ModelSpecifications
{
    public class MeasureDimension
    {
        public int RowId { get; set; }
        public string Name { get; set; }
        public string SystemKeyword { get; set; }
        public string Ratio { get; set; }
        public int DisplayOrder { get; set; }
        public string Message { get; set; }
    }
}
