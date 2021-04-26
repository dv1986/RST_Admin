using System;
using System.Collections.Generic;
using System.Text;

namespace ModelCategories
{
    public class ProductFeatures
    {
		public int RowId { get; set; }
		public string FeatureName { get; set; }
		public string Description { get; set; }
		public int ProductFeatureCategoryId { get; set; }
		public string CategoryShortName { get; set; }
		public string CategoryLongName { get; set; }
        public string Message { get; set; }
    }
}
