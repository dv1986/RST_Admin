using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class ProductAttribute
    {
        public int RowId { get; set; }
        public int ProductAttributeParentId { get; set; }
        public string AttributeName { get; set; }
        public string ProductAttributeParentName { get; set; }
        public string Message { get; set; }
    }
}

