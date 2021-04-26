using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class ProductSuggestion
    {
        public string ProductTitle { get; set; }
        public int ProductTypeId { get; set; }
        public int SubCategoryId { get; set; }
        public int SubCategoryParentId { get; set; }
        public int CategoryId { get; set; }
        public string ProductTypeName { get; set; }
        public string SubCategoryParentName { get; set; }
        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }

    }
}
