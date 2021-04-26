using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class ProductFilterRequest
    {
        public string ProductName { get; set; }
        public string year { get; set; }
        public string categories { get; set; }
        public int PriceRangeMin { get; set; }
        public int PriceRangeMax { get; set; }
        public int CategoryId { get; set; }
        public int ParentSubCategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ProductTypeId { get; set; }
        public int BrandId { get; set; }
        public string SortBy { get; set; }
        public bool IsAsc { get; set; }
        public int PageNumber { get; set; }
        public int NoOfRecord { get; set; }
    }
}
