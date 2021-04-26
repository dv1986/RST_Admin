using System;
using System.Collections.Generic;
using System.Text;

namespace ModelCategories
{
    public class ProductSubCategory
    {
		public int RowId { get; set; }
		public int ProductSubCategoryParentId { get; set; }
		public string SubCategoryName { get; set; }
		public string Description { get; set; }
		public int SeoContentId { get; set; }
		public bool IncludeInTopMenu { get; set; }
		public bool IsNew { get; set; }
		public bool HasDiscountApplied { get; set; }
		public bool IsPublished { get; set; }
		public bool IsDeleted { get; set; }
		public string DisplayOrder { get; set; }
		public DateTime CreatedOnUtc { get; set; }
		public DateTime ModifiedOnUtc { get; set; }
		public int ProductImageId { get; set; }
		public string SubCategoryParent { get; set; }
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public string CategoryParentName { get; set; }
		public int CategoryParentId { get; set; }
		public string MetaTitle { get; set; }
		public string MetaKeyword { get; set; }
		public string ImageName { get; set; }
		public string ImagePath { get; set; }
		public string ThumbnailPath { get; set; }
		public string ImageFileStream { get; set; }
		public string Message { get; set; }
    }
}
