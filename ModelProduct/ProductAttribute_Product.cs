using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class ProductAttribute_Product
    {
        public int RowId { get; set; }
        public int ProductAttributeId { get; set; }
        public int ProductId { get; set; }
        public string TextPrompt { get; set; }
        public string AttributeParent { get; set; }
        public string AttributeName { get; set; }
        public string Message { get; set; }
    }
}
