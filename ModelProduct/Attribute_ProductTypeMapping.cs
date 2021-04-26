using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class Attribute_ProductTypeMapping
    {
        public int? CategoryParentId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryParentId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? ProductTypeId { get; set; }
        public List<int> AttributeIds { get; set; }
    }
}
