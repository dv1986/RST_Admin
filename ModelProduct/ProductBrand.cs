using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class ProductBrand
    {
        public int RowId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryid { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string BrandName { get; set; }
        public bool Statue { get; set; }
        public string Message { get; set; }
    }
}
