using System;
using System.Collections.Generic;
using System.Text;

namespace ModelSpecifications
{
    public class ProductSizeType
    {
		public int RowId { get; set; }
		public string TypeName { get; set; }
		public string TypeCode { get; set; }
		public int MeasureDimensionId { get; set; }
		public DateTime CreatedOnUtc { get; set; }
		public string Description { get; set; }
		public string MeasurementDimension { get; set; }
        public string Message { get; set; }
    }
}
