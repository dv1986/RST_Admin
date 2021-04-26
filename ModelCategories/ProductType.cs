using System;
using System.Collections.Generic;
using System.Text;

namespace ModelCategories
{
    public class ProductType
    {
		public int RowId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IncludeInTopMenu { get; set; }
		public bool IsNew { get; set; }
		public bool HasDiscountApplied { get; set; }
		public bool IsPublished { get; set; }
		public bool IsDeleted { get; set; }
		public string DisplayOrder { get; set; }
		public DateTime CreatedOnUtc { get; set; }
		public DateTime ModifiedOnUtc { get; set; }
		public int ProductSizeTypeId { get; set; }
		public int ProductCategoryId { get; set; }
		public string CategoryName { get; set; }
		public int ProductSubCategoryParentId { get; set; }
		public string SubCategoryParentName { get; set; }
		public int ProductSubCategoryId { get; set; }
		public string SubCategoryName { get; set; }
		public int SeoContentId { get; set; }
		public string MetaTitle { get; set; }
		public string MetaKeyword { get; set; }
        public string TypeName { get; set; }
        public string Message { get; set; }
    }
}
