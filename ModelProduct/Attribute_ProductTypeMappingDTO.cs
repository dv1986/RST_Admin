using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class Attribute_ProductTypeMappingDTO
    {
        public int RowId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public int? ProductSubCategoryParentId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? ProductCategoryParentId { get; set; }
        public int ProductAttributeId { get; set; }
        public string AttributeName { get; set; }
        public string ProductTypeName { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryParentName { get; set; }
        public string CategoryName { get; set; }
        public string CategoryParentName { get; set; }
    }
}
