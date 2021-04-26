using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class FilteredProduct
    {
		public int RowId { get; set; }
		public string ProductTitle { get; set; }
		public string ShortDescription { get; set; }
		public string SKUCode { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public decimal ProductMRP { get; set; }
		public string ThumbnailImage { get; set; }
        public string Address { get; set; }
    }
}
